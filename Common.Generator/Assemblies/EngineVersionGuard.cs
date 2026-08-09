using System.Reflection;
using KY.Core;

namespace KY.Generator;

/// <summary>
/// Two engine versions in one process do not fail where they meet. They fail much later, when a type of the one
/// version is looked up in the assembly of the other, with a <see cref="ReflectionTypeLoadException" /> that names a
/// type but never a version - e.g. "Could not load type 'KY.Generator.ITypeScriptModelSyntax' from assembly
/// 'KY.Generator.TypeScript.Fluent, Version=10.0.1.0'". This guard stops the load where the second version enters and
/// reports both versions and both paths instead.
/// </summary>
/// <remarks>
/// Not every KY.Generator assembly moves with the engine - modules like KY.Generator.OData ship their own version and
/// request their own generator with it (see <see cref="GenerateWithAttribute.UseSameVersion" />). So only two things
/// are a mismatch: a resolved assembly that is not the version its caller asked for, and a core assembly that is not
/// the version of the running engine.
/// </remarks>
public class EngineVersionGuard
{
    private const string EnginePrefix = "KY.Generator";

    /// <summary>
    /// The assemblies that make up the engine itself. They are shipped together and every module is compiled against
    /// them, so a second version of one of them never works, no matter who asked for it.
    /// </summary>
    public static List<string> CoreAssemblies { get; } =
    [
        "KY.Generator.Common",
        "KY.Generator.Common.Generator",
        "KY.Generator.Common.Fluent"
    ];

    public string ReferenceName { get; }
    public Version? ReferenceVersion { get; }
    public string ReferencePath { get; }

    public EngineVersionGuard()
        : this(typeof(EngineVersionGuard).Assembly)
    { }

    public EngineVersionGuard(Assembly reference)
        : this(reference.GetName().Name, reference.GetName().Version, reference.Location)
    { }

    public EngineVersionGuard(string referenceName, Version? referenceVersion, string referencePath)
    {
        this.ReferenceName = referenceName;
        this.ReferenceVersion = referenceVersion;
        this.ReferencePath = referencePath;
    }

    public static bool IsEngineAssembly(string? name)
    {
        return name != null
               && (name.Equals(EnginePrefix, StringComparison.OrdinalIgnoreCase)
                   || name.StartsWith(EnginePrefix + ".", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns the message describing the version mismatch, or null if the assembly may be loaded.
    /// </summary>
    /// <param name="name">Name of the assembly that is about to be loaded</param>
    /// <param name="requestedVersion">Version the caller asked for, or null if it asked for any version</param>
    /// <param name="foundVersion">Version of the assembly the locators resolved to</param>
    /// <param name="foundPath">Path of the assembly the locators resolved to</param>
    public string? Validate(string? name, Version? requestedVersion, Version? foundVersion, string? foundPath)
    {
        if (!IsEngineAssembly(name) || foundVersion == null)
        {
            return null;
        }
        if (requestedVersion != null && !requestedVersion.Equals(foundVersion))
        {
            return this.Message($"{name} {requestedVersion} was requested, but only {name} {foundVersion} was found.", name, foundVersion, foundPath);
        }
        if (CoreAssemblies.Contains(name!) && this.ReferenceVersion != null && !this.ReferenceVersion.Equals(foundVersion))
        {
            return this.Message($"{name} {foundVersion} does not belong to the running engine {this.ReferenceVersion}.", name, foundVersion, foundPath);
        }
        return null;
    }

    /// <summary>
    /// Reports the version mismatch and stops the generation. Does nothing if the assembly may be loaded.
    /// </summary>
    public void Check(string? name, Version? requestedVersion, Version? foundVersion, string? foundPath)
    {
        string? message = this.Validate(name, requestedVersion, foundVersion, foundPath);
        if (message == null)
        {
            return;
        }
        // Logged as well as thrown: whoever catches the exception on the way out (the module loader does) would
        // otherwise report it without the detail that makes it actionable
        Logger.Error(message);
        throw new EngineVersionMismatchException(message);
    }

    private string Message(string headline, string? name, Version foundVersion, string? foundPath)
    {
        return $"KY.Generator engine version mismatch. {headline}\n"
               + "Two engine versions in one process fail later with an unrelated 'could not load type' error, so the generation is stopped here.\n"
               + $"  running: {this.ReferenceName} {this.ReferenceVersion}\n"
               + $"           {this.ReferencePath}\n"
               + $"  loading: {name} {foundVersion}\n"
               + $"           {foundPath}\n"
               + "Pin every KY.Generator* PackageReference to the same version. An 'Exe' project copies its pinned "
               + "packages to the output folder, which is searched before the NuGet cache, while assemblies loaded "
               + "before the output folder is known come from the newest version in the cache.";
    }
}
