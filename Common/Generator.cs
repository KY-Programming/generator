using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Extension;
using KY.Core.Module;
using KY.Core.Nuget;
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Licensing;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Settings;
using KY.Generator.Statistics;
using KY.Generator.Syntax;
using KY.Generator.Templates;
using KY.Generator.Transfer.Writers;

namespace KY.Generator;

public class Generator : IGeneratorRunSyntax
{
    private readonly IOutput output;
    private readonly DependencyResolver resolver;
    private readonly List<IGeneratorCommand> commands = [];
    private readonly GeneratorEnvironment environment = new();
    private readonly StatisticsService statisticsService;
    private readonly LicenseService licenseService;
    private readonly AssemblyCache assemblyCache;
    private bool initializationFailed;
    private readonly List<string> initializationErrors = [];

    public Generator()
    {
        DateTime start = DateTime.Now;
        Logger.Added += this.OnLoggerAdded;
        Assembly callingAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        FrameworkName framework = callingAssembly.GetTargetFramework();
        Logger.Trace($"KY-Generator v{callingAssembly.GetName().Version} ({framework.Identifier.Replace("App", string.Empty)} {framework.Version.Major}.{framework.Version.Minor})");
        Logger.Trace("Current Directory: " + Environment.CurrentDirectory);
        Logger.Trace("Log Directory: " + Logger.File.Path);

        Stopwatch prepareEnvironmentStopwatch = new();
        prepareEnvironmentStopwatch.Start();
        this.PrepareEnvironment();
        prepareEnvironmentStopwatch.Stop();
        Logger.Trace($"Prepared environment in {prepareEnvironmentStopwatch.ElapsedMilliseconds} ms");

        Stopwatch runtimeStopwatch = new();
        runtimeStopwatch.Start();
        this.assemblyCache = new AssemblyCache(this.environment);
        runtimeStopwatch.Stop();
        Logger.Trace($"Installed runtimes searched in {runtimeStopwatch.ElapsedMilliseconds} ms");

        NugetPackageDependencyLoader.Activate(this.assemblyCache);
        NugetPackageDependencyLoader.ResolveDependencies(this.GetType().Assembly);

        this.resolver = new DependencyResolver();
        this.resolver.Bind<AssemblyCache>().To(this.assemblyCache);
        this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
        this.resolver.Bind<GeneratorCommandFactory>().ToSingleton();
        this.resolver.Bind<GeneratorCommandRunner>().ToSelf();
        this.resolver.Bind<ModuleFinder>().ToSingleton();
        this.resolver.Bind<ModelWriter>().ToSelf();
        this.resolver.Bind<IEnvironment>().To(this.environment);
        this.output = new FileOutput(this.resolver.Get<IEnvironment>(), Environment.CurrentDirectory);
        this.resolver.Bind<IOutput>().To(this.output);
        this.resolver.Bind<List<FileTemplate>>().To([]);
        this.resolver.Bind<StatisticsService>().ToSingleton();
        this.resolver.Bind<GlobalStatisticsService>().ToSingleton();
        this.resolver.Bind<GlobalSettingsService>().ToSingleton();
        this.resolver.Bind<GlobalLicenseService>().ToSingleton();
        this.resolver.Bind<LicenseService>().ToSingleton();
        this.licenseService = this.resolver.Get<LicenseService>();
        this.licenseService.Check();

        Logger.Added -= this.OnLoggerAdded;
        this.statisticsService = this.resolver.Get<StatisticsService>();
        this.statisticsService.ProgramStart(start);
        this.statisticsService.Data.Errors.AddRange(this.initializationErrors);

        ModuleFinder moduleFinder = this.resolver.Get<ModuleFinder>();
        this.InitializeModules(moduleFinder.Modules);
    }

    private void PrepareEnvironment()
    {
        try
        {
            FileSystem.CreateDirectory(this.environment.ApplicationData);
        }
        catch (Exception exception)
        {
            Logger.Error($"Could not prepare environment. Could not create directory {this.environment.ApplicationData}. {exception.Message}");
            this.initializationFailed = true;
        }
        try
        {
            FileSystem.CreateDirectory(this.environment.LocalApplicationData);
        }
        catch (Exception exception)
        {
            Logger.Error($"Could not prepare environment. Could not create directory {this.environment.LocalApplicationData}. {exception.Message}");
            this.initializationFailed = true;
        }
    }

    public static Generator Create()
    {
        return new Generator();
    }

    public Generator SharedAssemblies(string sharedPath)
    {
        NugetPackageDependencyLoader.Locations.Insert(0, new SearchLocation(sharedPath) /*.Local()*/.SearchOnlyLocal());
        return this;
    }

    public Generator PreloadModules(string path, string? moduleFileNameSearchPattern = default)
    {
        ModuleFinder moduleFinder = this.resolver.Get<ModuleFinder>();
        List<ModuleBase> loadedModules = moduleFinder.LoadFrom(path, moduleFileNameSearchPattern);
        this.InitializeModules(loadedModules);
        return this;
    }

    public Generator PreloadModule<T>() where T : ModuleBase
    {
        return this;
    }

    public Generator SetOutput(string path)
    {
        this.output.Move(path);
        return this;
    }

    public Generator RegisterCommand<T>(IEnumerable<string> names) where T : IGeneratorCommand
    {
        this.resolver.Get<GeneratorCommandFactory>().Register<T>(names);
        return this;
    }

    public Generator RegisterCommand(Type command, IEnumerable<string> names)
    {
        this.resolver.Get<GeneratorCommandFactory>().Register(command, names);
        return this;
    }

    public IGeneratorRunSyntax ParseAttributes(string assemblyName)
    {
        Logger.Trace($"Read attributes from assembly {assemblyName}");
        throw new NotImplementedException();
        // List<IGeneratorCommand> generatorCommands = this.resolver.Get<GeneratorCommandFactory>().Create("RunByAttributes");
        // foreach (IGeneratorCommand command in generatorCommands)
        // {
        //     command.Parse(
        //         new CliCommandParameter("assembly", assemblyName),
        //         new CliCommandParameter("SkipAsyncCheck", bool.TrueString)
        //     );
        //     this.commands.Add(command);
        // }
        return this;
    }

    public IGeneratorRunSyntax SetParameters(params string[] parameters)
    {
        if (parameters.Length == 0)
        {
            Logger.Error("No parameters found. Provide at least a command or a path to a configuration file. Generation aborted!");
            GeneratorErrors.CommandDocumentationHint();
            return this;
        }
        Logger.Trace("Parameters: " + string.Join(" ", parameters));

        List<string> commandsStringsWithParameters = [];
        foreach (string parameter in parameters)
        {
            if (parameter.StartsWith("-*"))
            {
                this.environment.Parameters.Add(CliCommandParameter.Parse(parameter));
                parameter.Remove(parameter);
            }
            else
            {
                commandsStringsWithParameters.Add(parameter);
            }
        }

        List<CliCommand> cliCommands = CliCommandReader.Read(commandsStringsWithParameters.ToArray());
        this.commands.AddRange(this.resolver.Get<GeneratorCommandFactory>().Create(cliCommands));
        return this;
    }

    public bool Run()
    {
        this.statisticsService.InitializationEnd();
        bool success = true;
        try
        {
            if (this.initializationFailed)
            {
                success = false;
            }
            List<ILanguage> languages = this.resolver.Get<List<ILanguage>>();
            GeneratorCommand.AddParser(value => languages.FirstOrDefault(x => x.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase)));
            GeneratorCommandRunner runner = this.resolver.Get<GeneratorCommandRunner>();
            List<IGeneratorCommand> asyncCommands = [];
            IGeneratorCommandResult? switchContext = null;
            List<FileTemplate> files = [];
            bool switchAsync = false;
            this.commands.Sort((left, right) => left is IPrepareCommand && right is IPrepareCommand ? 0 : left is IPrepareCommand ? -1 : 1);
            foreach (IGeneratorCommand command in this.commands)
            {
                command.Parse();
                command.Prepare();
            }
            this.statisticsService.Data.IsMsBuild = this.statisticsService.Data.IsMsBuild || this.commands.Any(x => x.Parameters.IsMsBuild);
            this.statisticsService.Data.IsBeforeBuild = this.statisticsService.Data.IsBeforeBuild || this.commands.Any(x => x.Parameters.IsBeforeBuild);
            if (success || this.commands.Any(x => x.Parameters.Force))
            {
                foreach (IGeneratorCommand command in this.commands)
                {
                    IGeneratorCommandResult result = runner.Run(command);
                    success &= result.Success;
                    switchAsync = switchAsync || result.SwitchToAsync;
                    if (result.SwitchContext)
                    {
                        switchContext ??= result;
                    }
                    if (result.SwitchContext || result.SwitchToAsync || result.RerunOnAsync)
                    {
                        asyncCommands.Add(command);
                    }
                }
                this.statisticsService.RunEnd(this.environment.OutputId, this.environment.Name);
                files = this.resolver.Get<List<FileTemplate>>();
                if (files.Count > 0)
                {
                    this.licenseService.WaitOrKill();
                    if (this.licenseService.IsValid)
                    {
                        Logger.Trace("Generate code...");
                        files.Write(this.output, this.resolver);
                        this.statisticsService.GenerateEnd(this.output.Lines);
                        files.ForEach(file => this.statisticsService.Count(file));
                    }
                    else if (this.licenseService.ValidUntil > DateTime.MinValue)
                    {
                        Logger.Error("License has expired. Ensure that https://generator.ky-programming.de is reachable or generate a new offline license at https://generator.ky-programming.de/license");
                        Logger.Error("Generate code canceled!");
                        success = false;
                    }
                    else
                    {
                        Logger.Error("No valid license found. Ensure that https://generator.ky-programming.de is reachable or generate an offline license at https://generator.ky-programming.de/license");
                        Logger.Error("Generate code canceled!");
                        success = false;
                    }
                }
            }
            if (success)
            {
                this.output.Execute();
                this.commands.ForEach(command => command.FollowUp());
            }
            else
            {
                this.statisticsService.RunFailed();
            }
            this.assemblyCache.Save();
            if (switchAsync)
            {
                return this.SwitchToAsync(asyncCommands);
            }
            if (switchContext != null)
            {
                return this.SwitchContext(switchContext, asyncCommands);
            }
            this.statisticsService.ProgramEnd(files.Count);
            this.licenseService.ShowMessages();
        }
        catch (Exception exception)
        {
            Logger.Error(exception);
            success = false;
        }
        if (!success)
        {
            string message = "\n\n>>> NEED HELP?\n" +
                             ">>> check https://generator.ky-programming.de\n" +
                             $">>> or contact support{(char)64}ky-programming.de\n\n";
            if (Logger.ErrorTargets.Contains(Logger.MsBuildOutput))
            {
                message += $"\nSee the full log in: {Logger.File.Path}";
            }
            Logger.Error(message);
        }
        try
        {
            if (this.commands.Any() && !this.commands.OfType<StatisticsCommand>().Any() && this.resolver.Get<GlobalSettingsService>().Read().StatisticsEnabled)
            {
                string fileName = this.statisticsService.Write();
                this.resolver.Get<GlobalStatisticsService>().StartCalculation(fileName);
            }
        }
        catch (Exception exception)
        {
            Logger.Error(exception);
        }
        Logger.Trace("===============================");
        return success;
    }

    private void OnLoggerAdded(object sender, EventArgs<LogEntry> args)
    {
        if (args.Value.Type != LogType.Trace)
        {
            this.initializationErrors.Add(args.Value.Message);
        }
    }

    private bool SwitchContext(IGeneratorCommandResult result, IEnumerable<IGeneratorCommand> commandsToRun)
    {
        if (result.SwitchToArchitecture != null && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // TODO: Check other possibilities to run x86 in not Windows environments
            Logger.Error($"Can not start {result.SwitchToArchitecture} process. Your system does not support this process type.");
            return false;
        }
        if (result.SwitchToArchitecture != null)
        {
            Logger.Trace($"Different assembly architecture found. Switching to {result.SwitchToArchitecture}...");
        }
        if (result.SwitchToFramework != SwitchableFramework.None)
        {
            Logger.Trace($"Different assembly framework found. Switching to {result.SwitchToFramework}...");
        }
        string location = Assembly.GetEntryAssembly()?.Location ?? throw new InvalidOperationException("No location found");
        Regex regex = new(@"(?<separator>[\\/])(?<framework>net[^\\/]+)[\\/]");
        Match match = regex.Match(location);
        if (!match.Success)
        {
            throw new InvalidOperationException($"Invalid location {location}. Location has to include the framework to switch the context");
        }
        string framework = match.Groups["framework"].Value;
        string separator = match.Groups["separator"].Value;
        string switchedFramework = (result.SwitchToFramework.FrameworkName() ?? framework)
                                   + (result.SwitchToArchitecture != null ? $"-{result.SwitchToArchitecture.ToString().ToLower()}" : "");
        location = location.Replace(separator + framework + separator, separator + switchedFramework + separator);
        string locationExe = location.Replace(".dll", ".exe");
        if (FileSystem.FileExists(location) || FileSystem.FileExists(locationExe))
        {
            string arguments = string.Empty;
            if (result.SwitchToArchitecture != null)
            {
                arguments += $" --switchedFromArchitecture=\"{result.SwitchToArchitecture}\"";
            }
            if (result.SwitchToFramework != SwitchableFramework.None)
            {
                arguments += $" --switchedFromFramework=\"{result.SwitchToFramework}\"";
            }
            Logger.Trace("===============================");
            Process process = GeneratorProcess.Start(location, commandsToRun, arguments);
            process.WaitForExit();
            Logger.Trace($"{result.SwitchToArchitecture?.ToString() ?? result.SwitchToFramework.ToString()} process exited with code {process.ExitCode}");
            return process.ExitCode == 0;
        }
        Logger.Error($"Can not start {result.SwitchToArchitecture} process. File \"{location}\" not found. Try to update to .net Core Framework 3.0 or later.");
        return false;
    }

    private bool SwitchToAsync(IEnumerable<IGeneratorCommand> commandsToRun)
    {
        Logger.Trace($"The generation is continued in a separate asynchronous process. You can find the output log here: {Logger.File.Path}");
        GeneratorProcess.StartHidden(commandsToRun, " -*only-async");
        return true;
    }

    public static void InitializeLogger(string[] parameters)
    {
        Logger.CatchAll();
        Logger.Console.ShortenEntries = false;
        Logger.AllTargets.Add(Logger.VisualStudioOutput);
        if (parameters.Any(parameter => parameter.ToLowerInvariant().Contains("forwardlogging")))
        {
            ForwardConsoleTarget target = new();
            Logger.AllTargets.Clear();
            Logger.AllTargets.Add(target);
            Logger.TraceTargets.Clear();
            Logger.TraceTargets.Add(target);
            Logger.ErrorTargets.Clear();
            Logger.ErrorTargets.Add(target);
        }
        if (parameters.Any(parameter => parameter?.EndsWith("msbuild", StringComparison.CurrentCultureIgnoreCase) ?? false))
        {
            Logger.Trace("MsBuild trace mode activated");
            Logger.WarningTargets.Add(Logger.MsBuildOutput);
            Logger.ErrorTargets.Add(Logger.MsBuildOutput);
            Logger.WarningTargets.Remove(Logger.VisualStudioOutput);
            Logger.ErrorTargets.Remove(Logger.VisualStudioOutput);
        }
    }

    private void InitializeModules(IEnumerable<ModuleBase> modules)
    {
        List<ModuleBase> list = modules.ToList();
        Dictionary<ModuleBase, Stopwatch> stopwatches = list.ToDictionary(x => x, _ => new Stopwatch());
        this.statisticsService.Data.InitializedModules = list.Count;
        foreach (ModuleBase module in list)
        {
            stopwatches[module].Start();
            this.resolver.Bind<ModuleBase>().To(module);
            stopwatches[module].Stop();
        }
        foreach (ModuleBase module in list)
        {
            Stopwatch stopwatch = stopwatches[module];
            stopwatch.Start();
            module.Initialize();
            stopwatch.Stop();
            Logger.Trace($"{module.GetType().Name.Replace("Module", "")}-{module.GetType().Assembly.GetName().Version} module loaded in {(stopwatch.ElapsedMilliseconds >= 1 ? stopwatch.ElapsedMilliseconds.ToString() : "<1")} ms");
        }
    }
}
