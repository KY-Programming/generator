using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Extension;
using KY.Generator.Extensions;
using KY.Generator.Models;
using Newtonsoft.Json.Linq;

namespace KY.Generator;

/// <summary>
/// Reads the <c>ky-generator.json</c> files that apply to the project being generated.
/// <para>
/// The search starts in the directory <em>above</em> the project and walks up to the root of the drive: at project
/// level the assembly attributes are the way to configure the generator, a settings file next to the project would
/// only be a second way to say the same thing. The file in the application data folder is the outermost layer of the
/// chain, so a file found in the tree always wins over it, and every file wins over the one above it.
/// </para>
/// <para>
/// The result is written to the global options, which is the root of the option tree. Everything the assembly
/// attributes, the fluent syntax or the CLI set is closer to the generated type and therefore wins over it
/// </para>
/// </summary>
public class SettingsService
{
    public const string FileName = "ky-generator.json";
    public const string DefaultApi = "https://generator.ky-programming.de";

    /// <summary>
    /// The schema a settings file names in its <c>$schema</c>, so an editor can complete and validate it. The major
    /// version is part of it: a file written for this generator has to keep validating once the next one is out
    /// </summary>
    public static string SchemaUrl { get; } = $"{DefaultApi}/v{typeof(SettingsService).Assembly.GetName().Version?.Major ?? 10}/{Commands.SettingsSchemaCommand.FileName}";

    /// <summary>
    /// What the versions before the settings files kept in the application data folder. It is read once, to carry
    /// the installation id and the statistics of an existing installation over, and then left alone - an older
    /// generator on the same machine still reads it
    /// </summary>
    private const string LegacyFileName = "global.settings.json";

    /// <summary>
    /// The keys <see cref="GeneratorSettings"/> owns. Everything else on the top level has to be the section of an
    /// <see cref="IConfigurableOptionsFactory"/>
    /// </summary>
    private static readonly string[] ownKeys =
    [
        GeneratorSettings.SchemaKey, ..GeneratorSettings.Options.Select(option => option.Name)
    ];

    private static readonly JsonMergeSettings mergeSettings = new()
    {
        MergeArrayHandling = MergeArrayHandling.Replace,
        MergeNullValueHandling = MergeNullValueHandling.Ignore
    };

    /// <summary>
    /// One resolved chain per start directory. A run that generates for more than one project walks a shared
    /// ancestor only once
    /// </summary>
    private static readonly Dictionary<string, ResolvedSettings> cache = new(StringComparer.OrdinalIgnoreCase);

    private readonly IEnvironment environment;
    private readonly IDependencyResolver resolver;
    private string? startPath;
    private ResolvedSettings? resolved;
    private Guid? license;
    private bool enabled;
    private bool warned;

    /// <summary>
    /// The directory the search starts in. Defaults to the current directory for a run that has no project
    /// </summary>
    public string StartPath
    {
        get => this.startPath ??= Environment.CurrentDirectory;
        set
        {
            this.startPath = value;
            this.resolved = null;
            this.license = null;
        }
    }

    /// <summary>
    /// The files that apply, outermost first
    /// </summary>
    public IReadOnlyList<SettingsFile> Files => this.Resolved.Files;

    public string Api
    {
        get
        {
            string api = this.Resolved.Settings.Api?.TrimEnd('/') ?? string.Empty;
            return string.IsNullOrWhiteSpace(api) ? DefaultApi : api;
        }
    }

    public bool Statistics => this.Resolved.Settings.Statistics ?? true;

    /// <summary>
    /// The license the run works with: the one a settings file names, or the id of this installation
    /// </summary>
    public Guid License => this.license ??= this.Resolved.Settings.License ?? this.GetOrCreateInstallationLicense();

    public string? Certificate => string.IsNullOrWhiteSpace(this.Resolved.Settings.Certificate) ? null : this.Resolved.Settings.Certificate;

    private ResolvedSettings Resolved => this.resolved ??= Resolve(this.StartPath, this.environment.ApplicationData);

    public SettingsService(IEnvironment environment, IDependencyResolver resolver)
    {
        this.environment = environment;
        this.resolver = resolver;
    }

    /// <summary>
    /// Points the search at the project that is generated. The search starts above it, see <see cref="SettingsService"/>
    /// </summary>
    public void SetProject(string projectFilePath)
    {
        if (string.IsNullOrWhiteSpace(projectFilePath))
        {
            return;
        }
        string projectDirectory = FileSystem.Parent(ToAbsolute(projectFilePath));
        string? parent = ParentOrNull(projectDirectory);
        if (parent == null)
        {
            // A project directly in the root of a drive has nothing above it. Reading its own directory instead
            // would break the rule that a settings file never sits next to the project it configures
            Logger.Trace($"No directory above the project {projectDirectory}. No settings file is read");
            this.startPath = projectDirectory;
            this.resolved = ResolvedSettings.Empty;
            return;
        }
        this.StartPath = parent;
    }

    /// <summary>
    /// Lets the settings be applied. Called once the parameters are read, because they name the project the settings
    /// are searched for - applying before that would write the settings of the current directory
    /// </summary>
    public void Enable()
    {
        this.enabled = true;
    }

    /// <summary>
    /// Writes every section to the global options of the factory that owns it. Runs again whenever a module is
    /// loaded: the modules bring the sections with them, and they are loaded lazily while the commands run. Writing
    /// the same value a second time changes nothing, so running it more than once is free
    /// </summary>
    public void Apply()
    {
        if (!this.enabled)
        {
            return;
        }
        ResolvedSettings settings = this.Resolved;
        if (settings.Files.Count == 0)
        {
            return;
        }
        foreach (IConfigurableOptionsFactory factory in this.GetFactories())
        {
            if (Get(settings.Merged, factory.SettingsSection) is not JObject section)
            {
                continue;
            }
            object options = Options.GetGlobal(factory.OptionsType);
            foreach (JProperty property in section.Properties())
            {
                SettingsOption? option = factory.SettingsOptions.FirstOrDefault(x => x.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                if (option == null)
                {
                    continue;
                }
                try
                {
                    option.Apply(options, property.Value);
                }
                catch (Exception exception)
                {
                    Logger.Warning($"Could not apply '{factory.SettingsSection}.{option.Name}' from {settings.GetSource($"{factory.SettingsSection}.{option.Name}") ?? FileName}. {exception.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Reports every key that no module claims. Runs at the end of a run, because the modules are loaded lazily and
    /// a section would look unknown earlier on only because the module that owns it is not loaded yet.
    /// <para>
    /// Pass <paramref name="allModulesLoaded"/> only from a command that loaded every module that ships. Without it
    /// an unknown section is reported as a trace and not as a warning: a run that generates nothing but C# never
    /// loads the TypeScript module, and a <c>typescript</c> section is perfectly fine in the file it reads
    /// </para>
    /// </summary>
    public void WarnAboutUnknownKeys(bool allModulesLoaded = false)
    {
        if (this.warned || !this.enabled)
        {
            return;
        }
        this.warned = true;
        this.WarnAboutUnknownKeys(this.Resolved, this.GetFactories(), allModulesLoaded);
    }

    /// <summary>
    /// The file that set the value of a key, e.g. <c>options.addHeader</c>, or <c>null</c> if no file did
    /// </summary>
    public string? GetSource(string key)
    {
        return this.Resolved.GetSource(key);
    }

    public List<IConfigurableOptionsFactory> GetFactories()
    {
        return this.resolver.TryGet<List<IOptionsFactory>>()?.OfType<IConfigurableOptionsFactory>().ToList() ?? [];
    }

    /// <summary>
    /// The path of the file <c>settings-set</c> and <c>settings-init</c> write to
    /// </summary>
    public string GetWritablePath(bool global, string? directory)
    {
        if (global)
        {
            return this.GlobalPath;
        }
        if (!string.IsNullOrWhiteSpace(directory))
        {
            return FileSystem.Combine(ToAbsolute(directory!), FileName);
        }
        return this.Files.LastOrDefault(file => !file.IsGlobal)?.Path ?? FileSystem.Combine(this.StartPath, FileName);
    }

    /// <summary>
    /// Writes a value into the settings of this machine, for the commands that change a setting instead of reading
    /// one. Nothing else in the generator writes a settings file - a build only ever reads them
    /// </summary>
    public void SetGlobal(string key, JToken value)
    {
        string path = this.GlobalPath;
        JObject content = SettingsWriter.Load(path);
        SettingsWriter.Set(content, key, value);
        SettingsWriter.Save(path, content);
        this.resolved = null;
        this.license = null;
        ClearCache();
    }

    /// <summary>
    /// The option a dotted key like <c>options.formatting.indentCount</c> names, or <c>null</c> if it names none.
    /// The keys of <see cref="GeneratorSettings"/> have no option and are reported by <paramref name="isOwnKey"/>
    /// </summary>
    public SettingsOption? FindOption(string key, out bool isOwnKey)
    {
        string[] segments = key.Split('.');
        isOwnKey = segments.Length == 1 && ownKeys.Any(own => own.Equals(segments[0], StringComparison.OrdinalIgnoreCase));
        if (isOwnKey || segments.Length < 2)
        {
            return null;
        }
        IConfigurableOptionsFactory? factory = this.GetFactories().FirstOrDefault(x => x.SettingsSection.Equals(segments[0], StringComparison.OrdinalIgnoreCase));
        IReadOnlyList<SettingsOption>? options = factory?.SettingsOptions;
        SettingsOption? option = null;
        foreach (string segment in segments.Skip(1))
        {
            option = options?.FirstOrDefault(x => x.Name.Equals(segment, StringComparison.OrdinalIgnoreCase));
            if (option == null)
            {
                return null;
            }
            options = option.Children;
        }
        return option;
    }

    /// <summary>
    /// Every key a settings file may carry, as the dotted form <c>settings-set</c> takes
    /// </summary>
    public IEnumerable<string> GetKnownKeys()
    {
        foreach (SettingsOption option in GeneratorSettings.Options)
        {
            yield return option.Name;
        }
        foreach (IConfigurableOptionsFactory factory in this.GetFactories().OrderBy(x => x.SettingsSection))
        {
            foreach (string key in GetKnownKeys(factory.SettingsSection, factory.SettingsOptions))
            {
                yield return key;
            }
        }
    }

    private string GlobalPath => FileSystem.Combine(this.environment.ApplicationData, FileName);

    /// <summary>
    /// The id that identifies this installation, created on first use. It is not part of the settings that are
    /// merged - it says which machine is generating, not how.
    /// <para>
    /// A settings file with <see cref="GeneratorSettings.IgnoreGlobalSettings"/> cuts the settings of the machine
    /// off, so the id can not be kept in them. It goes into a file of its own in the application data folder, named
    /// after the path of the settings file that opted out. That keeps it out of the repository - a build must never
    /// write into one - while staying stable per machine, instead of asking the api for a new id on every run
    /// </para>
    /// </summary>
    private Guid GetOrCreateInstallationLicense()
    {
        SettingsFile? ignoring = this.Resolved.Files.FirstOrDefault(file => file.IgnoresGlobalSettings);
        string path = ignoring == null ? this.GlobalPath : FileSystem.Combine(this.environment.ApplicationData, ToFileName(ignoring.Path));
        JObject content = SettingsWriter.Load(path);
        Guid? stored = GetGuid(content, nameof(GeneratorSettings.License));
        if (stored != null)
        {
            return stored.Value;
        }
        Guid created = Guid.NewGuid();
        try
        {
            SettingsWriter.Set(content, nameof(GeneratorSettings.License).ToCamelCase(), created.ToString());
            SettingsWriter.Save(path, content);
            Logger.Trace($"New installation id {created} written to {path}");
        }
        catch (Exception exception)
        {
            // Without the file the next run asks for a new id. That is slow, not broken, and a machine that can not
            // write to its own application data folder has bigger problems than a repeated license check
            Logger.Warning($"Could not write the installation id to {path}." + Environment.NewLine + exception.Message);
        }
        return created;
    }

    /// <summary>
    /// Turns the path of a settings file into a file name for the application data folder, e.g.
    /// <c>C:\Projekte\Generator\Tests\ky-generator.json</c> into <c>c-projekte-generator-tests-ky-generator.json</c>
    /// </summary>
    private static string ToFileName(string path)
    {
        string name = path.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? path.Substring(0, path.Length - 5) : path;
        string slug = Regex.Replace(name.ToLowerInvariant(), "[^a-z0-9]+", "-").Trim('-');
        // Windows stops at 255 characters, and a deeply nested repository gets close. The hash keeps two paths that
        // are cut to the same prefix apart
        return slug.Length <= 120 ? slug + ".json" : $"{slug.Substring(0, 120)}-{Hash(path)}.json";
    }

    /// <summary>
    /// FNV-1a. <see cref="string.GetHashCode()"/> is randomized per process, so it can not name a file that has to
    /// be found again by the next run
    /// </summary>
    private static string Hash(string value)
    {
        uint hash = 2166136261;
        foreach (byte current in Encoding.UTF8.GetBytes(value.ToLowerInvariant()))
        {
            hash = (hash ^ current) * 16777619;
        }
        return hash.ToString("x8");
    }

    private void WarnAboutUnknownKeys(ResolvedSettings settings, List<IConfigurableOptionsFactory> factories, bool allModulesLoaded)
    {
        foreach (SettingsFile file in settings.Files)
        {
            foreach (JProperty property in file.Content.Properties())
            {
                if (ownKeys.Any(key => key.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                IConfigurableOptionsFactory? factory = factories.FirstOrDefault(x => x.SettingsSection.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                if (factory == null)
                {
                    // Not an error either way: a file written for a newer generator has to keep working
                    string message = $"Unknown entry '{property.Name}' in {file.Path}";
                    if (allModulesLoaded)
                    {
                        Logger.Warning(message);
                    }
                    else
                    {
                        Logger.Trace($"{message}. No module of this run owns it - run '{Commands.SettingsShowCommandParameters.Names.First()}' to check the whole file");
                    }
                    continue;
                }
                if (property.Value is JObject section)
                {
                    WarnAboutUnknownKeys(file, factory.SettingsSection, section, factory.SettingsOptions);
                }
                else
                {
                    Logger.Warning($"Entry '{property.Name}' in {file.Path} has to be an object");
                }
            }
        }
    }

    private static void WarnAboutUnknownKeys(SettingsFile file, string path, JObject section, IReadOnlyList<SettingsOption> options)
    {
        foreach (JProperty property in section.Properties())
        {
            SettingsOption? option = options.FirstOrDefault(x => x.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
            if (option == null)
            {
                Logger.Warning($"Unknown option '{path}.{property.Name}' in {file.Path}");
                continue;
            }
            if (option.Children.Count > 0 && property.Value is JObject children)
            {
                WarnAboutUnknownKeys(file, $"{path}.{option.Name}", children, option.Children);
            }
        }
    }

    private static IEnumerable<string> GetKnownKeys(string path, IReadOnlyList<SettingsOption> options)
    {
        foreach (SettingsOption option in options)
        {
            if (option.Children.Count > 0)
            {
                foreach (string key in GetKnownKeys($"{path}.{option.Name}", option.Children))
                {
                    yield return key;
                }
                continue;
            }
            yield return $"{path}.{option.Name}";
        }
    }

    private static ResolvedSettings Resolve(string startPath, string applicationData)
    {
        lock (cache)
        {
            if (cache.TryGetValue(startPath, out ResolvedSettings cached))
            {
                return cached;
            }
            Stopwatch stopwatch = new();
            stopwatch.Start();
            ResolvedSettings settings = ReadChain(startPath, applicationData);
            stopwatch.Stop();
            if (settings.Files.Count > 0)
            {
                Logger.Trace($"Settings read from {string.Join(", ", settings.Files.Select(x => x.Path))} in {stopwatch.FormattedElapsed()}");
            }
            cache[startPath] = settings;
            return settings;
        }
    }

    /// <summary>
    /// Collects the files from the start directory upwards, then adds the one of the machine and merges them the
    /// other way around, so the file closest to the project overwrites what the ones above it set
    /// </summary>
    private static ResolvedSettings ReadChain(string startPath, string applicationData)
    {
        List<SettingsFile> files = [];
        string? directory = ToAbsolute(startPath);
        while (directory != null)
        {
            SettingsFile? file = Read(FileSystem.Combine(directory, FileName), false);
            if (file != null)
            {
                files.Add(file);
                if (file.IsRoot)
                {
                    break;
                }
            }
            directory = ParentOrNull(directory);
        }
        if (!files.Any(file => file.IgnoresGlobalSettings))
        {
            SettingsFile? global = Read(MigrateLegacyFile(applicationData), true);
            if (global != null)
            {
                files.Add(global);
            }
        }
        files.Reverse();
        JObject merged = new();
        Dictionary<string, string> sources = new(StringComparer.OrdinalIgnoreCase);
        foreach (SettingsFile file in files)
        {
            merged.Merge(file.Content, mergeSettings);
            foreach (JToken leaf in file.Content.Descendants().Where(token => token is JValue))
            {
                sources[leaf.Path] = file.Path;
            }
        }
        return new ResolvedSettings(files, merged, merged.ToObject<GeneratorSettings>() ?? new GeneratorSettings(), sources);
    }

    /// <summary>
    /// Carries the installation id and the statistics of a version that kept them in <c>global.settings.json</c>
    /// over into the settings of the machine. The old file is left alone, so an older generator installed next to
    /// this one keeps its installation id
    /// </summary>
    private static string MigrateLegacyFile(string applicationData)
    {
        string path = FileSystem.Combine(applicationData, FileName);
        string legacyPath = FileSystem.Combine(applicationData, LegacyFileName);
        if (FileSystem.FileExists(path) || !FileSystem.FileExists(legacyPath))
        {
            return path;
        }
        try
        {
            JObject legacy = JObject.Parse(FileSystem.ReadAllText(legacyPath));
            JObject content = new();
            Guid? license = GetGuid(legacy, "License");
            bool? statistics = Get(legacy, "StatisticsEnabled")?.Value<bool?>();
            if (license != null)
            {
                SettingsWriter.Set(content, nameof(GeneratorSettings.License).ToCamelCase(), license.Value.ToString());
            }
            if (statistics != null)
            {
                SettingsWriter.Set(content, nameof(GeneratorSettings.Statistics).ToCamelCase(), statistics.Value);
            }
            if (content.HasValues)
            {
                SettingsWriter.Save(path, content);
                Logger.Trace($"{legacyPath} carried over into {path}");
            }
        }
        catch (Exception exception)
        {
            Logger.Warning($"Could not read {legacyPath}. Continuing without it." + Environment.NewLine + exception.Message);
        }
        return path;
    }

    private static SettingsFile? Read(string path, bool isGlobal)
    {
        if (!FileSystem.FileExists(path))
        {
            return null;
        }
        try
        {
            // Comments are skipped by default, so a settings file may be written as jsonc
            JObject content = JObject.Parse(FileSystem.ReadAllText(path));
            return new SettingsFile(path, content,
                                    GetBool(content, nameof(GeneratorSettings.Root)),
                                    GetBool(content, nameof(GeneratorSettings.IgnoreGlobalSettings)),
                                    isGlobal);
        }
        catch (Exception exception)
        {
            Logger.Warning($"Could not read {path}. It is ignored." + Environment.NewLine + exception.Message);
            // Kept in the chain with no content: it changes nothing, and settings-validate has to report it
            return new SettingsFile(path, new JObject(), false, false, isGlobal, exception.Message);
        }
    }

    /// <summary>
    /// Reads a property without caring about its casing, like the deserialization of the file does
    /// </summary>
    private static JToken? Get(JObject content, string name)
    {
        return content.Properties().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Value;
    }

    private static bool GetBool(JObject content, string name)
    {
        return Get(content, name)?.Value<bool?>() == true;
    }

    /// <summary>
    /// A guid is a string in json, and <c>Value&lt;Guid?&gt;()</c> refuses to convert one - it throws instead of
    /// parsing. Reads it as text and reports anything unparseable as not set
    /// </summary>
    private static Guid? GetGuid(JObject content, string name)
    {
        return Guid.TryParse(Get(content, name)?.Value<string>(), out Guid parsed) && parsed != Guid.Empty ? parsed : null;
    }

    /// <summary>
    /// Resolves against the current directory, not against the application base directory the file system defaults
    /// to. The chain describes where the generated project sits, which has nothing to do with where the tool itself
    /// is installed
    /// </summary>
    private static string ToAbsolute(string path)
    {
        return FileSystem.IsAbsolute(path) ? FileSystem.FormatPath(path) : FileSystem.Combine(Environment.CurrentDirectory, path);
    }

    /// <summary>
    /// The parent directory, or <c>null</c> at the root of the drive
    /// </summary>
    private static string? ParentOrNull(string directory)
    {
        string parent = FileSystem.Parent(directory);
        return string.IsNullOrEmpty(parent) || parent.Equals(directory, StringComparison.OrdinalIgnoreCase) ? null : parent;
    }

    /// <summary>
    /// Drops every resolved chain. Only needed by the tests, which run more than one resolution in one process
    /// </summary>
    internal static void ClearCache()
    {
        lock (cache)
        {
            cache.Clear();
        }
    }

    private class ResolvedSettings(IReadOnlyList<SettingsFile> files, JObject merged, GeneratorSettings settings, Dictionary<string, string> sources)
    {
        public static ResolvedSettings Empty { get; } = new([], new JObject(), new GeneratorSettings(), new Dictionary<string, string>());

        public IReadOnlyList<SettingsFile> Files { get; } = files;
        public JObject Merged { get; } = merged;
        public GeneratorSettings Settings { get; } = settings;

        public string? GetSource(string key)
        {
            return sources.TryGetValue(key, out string source) ? source : null;
        }
    }
}
