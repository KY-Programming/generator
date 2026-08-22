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
using KY.Generator.Command;
using KY.Generator.Commands;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Licensing;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Statistics;
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
    private readonly GeneratorModuleLoader moduleLoader;
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
        Logger.Trace($"Prepared environment in {prepareEnvironmentStopwatch.FormattedElapsed()}");

        Stopwatch runtimeStopwatch = new();
        runtimeStopwatch.Start();
        runtimeStopwatch.Stop();
        Logger.Trace($"Installed runtimes searched in {runtimeStopwatch.FormattedElapsed()}");

        this.resolver = new DependencyResolver();
        this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
        this.resolver.Bind<GeneratorCommandFactory>().ToSingleton();
        this.resolver.Bind<GeneratorCommandRunner>().ToSelf();
        this.resolver.Bind<ModuleFinder>().ToSingleton();
        this.resolver.Bind<ModelWriter>().ToSelf();
        this.resolver.Bind<IEnvironment>().To(this.environment);
        this.output = new FileOutput(this.resolver.Get<IEnvironment>(), Environment.CurrentDirectory);
        this.resolver.Bind<IOutput>().To(this.output);
        this.resolver.Bind<List<FileTemplate>>().To([]);
        this.resolver.Bind<SettingsService>().ToSingleton();
        this.resolver.Bind<StatisticsService>().ToSingleton();
        this.resolver.Bind<GlobalStatisticsService>().ToSingleton();
        this.resolver.Bind<GlobalLicenseService>().ToSingleton();
        this.resolver.Bind<ILicenseService>().ToSingleton<LicenseService>();
        this.resolver.Bind<AssemblyLoader>().ToSingleton();
        this.resolver.Get<AssemblyLoader>().Activate();
        this.resolver.Bind<NativeAssetLocator>().ToSelf();
        this.resolver.Bind<NativeLibraryLoader>().ToSelf();
        this.resolver.Bind<GeneratorModuleLoader>().ToSingleton();
        this.moduleLoader = this.resolver.Get<GeneratorModuleLoader>();

        Logger.Added -= this.OnLoggerAdded;
        this.statisticsService = this.resolver.Get<StatisticsService>();
        this.statisticsService.ProgramStart(start);
        this.statisticsService.Data.Errors.AddRange(this.initializationErrors);

        this.moduleLoader.InitializeModules();
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

    public Generator PreloadModules(string path, string moduleFileNameSearchPattern)
    {
        this.moduleLoader.Load(path, moduleFileNameSearchPattern);
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
        //         new CliCommandParameter("SkipBackgroundCheck", bool.TrueString)
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
        this.SetSettingsStart(cliCommands);
        this.resolver.Get<SettingsService>().Enable();
        this.commands.AddRange(this.resolver.Get<GeneratorCommandFactory>().Create(cliCommands));
        return this;
    }

    /// <summary>
    /// The settings files are searched relative to the project that is generated, which is only known from the
    /// parameters. It has to be set before the first command runs, because the very first thing a command does can
    /// be to read an option
    /// </summary>
    private void SetSettingsStart(List<CliCommand> cliCommands)
    {
        string? project = cliCommands.SelectMany(command => command.Parameters)
                                     .FirstOrDefault(parameter => parameter.Name == CliCommandParameter.FormatName(nameof(ReadProjectCommandParameters.Project)))
                                     ?.Value;
        if (!string.IsNullOrWhiteSpace(project))
        {
            this.resolver.Get<SettingsService>().SetProject(project!);
        }
    }

    public async Task<bool> Run()
    {
        this.statisticsService.InitializationEnd();
        bool success = true;
        try
        {
            if (this.initializationFailed)
            {
                success = false;
            }
            // Has to run before the first command: it writes the settings files to the global options, which
            // are the root every other options object inherits from
            this.resolver.Get<SettingsService>().Apply();
            LicenseService licenseService = this.resolver.Get<LicenseService>();
            licenseService.ApplySettings();
            if (this.commands.All(x => x is not LoadCommand))
            {
                licenseService.Check();
            }
            GeneratorCommandRunner runner = this.resolver.Get<GeneratorCommandRunner>();
            IGeneratorCommandResult? switchContext = null;
            List<FileTemplate> files = [];
            bool switchToBackground = false;
            this.commands.Sort((left, right) => left is IPrepareCommand && right is IPrepareCommand ? 0 : left is IPrepareCommand ? -1 : 1);
            foreach (IGeneratorCommand command in this.commands)
            {
                command.Parse();
                command.Prepare();
            }
            List<ILanguage> languages = this.resolver.TryGet<List<ILanguage>>();
            GeneratorCommand.AddParser(value => languages.FirstOrDefault(x => x.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase)));
            if (success || this.environment.Force)
            {
                foreach (IGeneratorCommand command in this.commands)
                {
                    IGeneratorCommandResult result = await runner.Run(command);
                    success &= result.Success;
                    switchToBackground = switchToBackground || result.SwitchToBackground;
                    if (result.SwitchContext)
                    {
                        switchContext ??= result;
                    }
                    if (switchContext != null)
                    {
                        // The whole chain is executed again in the switched process. Running the rest of it here
                        // only duplicates the work and warns about the assemblies that the current context - the
                        // very reason for the switch - could not load.
                        break;
                    }
                    if (!result.Success)
                    {
                        break;
                    }
                }
                this.statisticsService.RunEnd(this.environment.OutputId, this.environment.Name);
                files = switchContext == null ? this.resolver.Get<List<FileTemplate>>() : [];
                if (files.Count > 0)
                {
                    licenseService.WaitOrKill();
                    if (licenseService.IsValid)
                    {
                        Logger.Trace("Generate code...");
                        files.Write(this.output, this.resolver);
                        this.statisticsService.GenerateEnd(this.output.Lines, files.Count);
                        files.ForEach(file => this.statisticsService.Count(file));
                    }
                    else if (licenseService.ValidUntil > DateTime.MinValue)
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
            if (success && switchContext == null)
            {
                this.output.Execute();
                this.commands.ForEach(command => command.FollowUp());
            }
            if (switchToBackground)
            {
                return this.SwitchToBackground(this.commands);
            }
            if (switchContext != null)
            {
                return this.SwitchContext(switchContext, this.commands);
            }
            licenseService.ShowMessages();
            if (success)
            {
                success = this.RunAtSuccess();
            }
        }
        catch (EngineVersionMismatchException)
        {
            success = false;
        }
        catch (Exception exception)
        {
            Logger.Error(exception);
            success = false;
        }
        finally
        {
            // At the end, because a module - and with it the section of the settings file it owns - can be
            // loaded by any command along the way
            this.resolver.Get<SettingsService>().WarnAboutUnknownKeys();
            this.statisticsService.ProgramEnd(success);
        }
        if (!success)
        {
            this.statisticsService.RunFailed(this.environment.OutputId, this.environment.Name);
            string message = "\n\n>>> NEED HELP?\n" +
                             ">>> check https://generator.ky-programming.de\n" +
                             $">>> or contact support{(char)64}ky-programming.de\n\n";
            if (Logger.ErrorTargets.Contains(Logger.MsBuildOutput))
            {
                message += $"\nSee the full log in: {Logger.File.Path}";
            }
            Logger.Error(message);
            this.NotifyBackgroundFailure();
        }
        try
        {
            if (this.commands.Any() && !this.commands.OfType<StatisticsCommand>().Any()
                && this.resolver.Get<SettingsService>().Statistics)
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

    /// <summary>
    /// The asynchronous run has nobody to report to - the build that started it has finished, and the process is
    /// hidden, so its console output goes nowhere. A desktop notification is the only channel left that reaches the
    /// developer without them having to know that a log file exists.
    /// </summary>
    private void NotifyBackgroundFailure()
    {
        if (this.commands.All(command => !command.Parameters.IsBackgroundRun))
        {
            return;
        }
        string name = string.IsNullOrEmpty(this.environment.Name) ? "A project" : this.environment.Name;
        DesktopNotification.ShowError("KY-Generator", $"{name}: the asynchronous code generation failed. See the log in {Logger.File.Path}");
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
                // arguments += $" --switchedFromArchitecture=\"{result.SwitchToArchitecture}\"";
            }
            if (result.SwitchToFramework != SwitchableFramework.None)
            {
                // arguments += $" --switchedFromFramework=\"{result.SwitchToFramework}\"";
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

    /// <summary>
    /// Hands the generation over to a detached process and returns immediately - the point of
    /// <c>[GenerateInBackground]</c> is that the build does not wait for it.
    /// <para>
    /// The whole chain is handed over, not only the commands that asked for the switch: the commands that ran
    /// before them - 'set', 'load', ... - are their prerequisites, and the detached process starts from scratch
    /// without them. 'msbuild' is the exception, there is no MSBuild listening to a process the build does not
    /// wait for.
    /// </para>
    /// <para>
    /// The current process throws its own result away (see the <c>switchContext</c> handling in <see cref="Run"/>),
    /// so everything this run would have generated is generated by the detached one.
    /// </para>
    /// </summary>
    private bool SwitchToBackground(IEnumerable<IGeneratorCommand> commandsToRun)
    {
        Logger.Trace($"The generation is continued in a separate asynchronous process. You can find the output log here: {Logger.File.Path}");
        GeneratorProcess.StartHidden(commandsToRun.Where(command => command is not MsBuildCommand), " -*background-run");
        return true;
    }

    /// <summary>
    /// Runs the command lines of the <see cref="RunAtSuccessAttribute"/> of every loaded assembly, after everything
    /// else is done and every file is written. Their purpose is the background generation: the build is long gone
    /// when that process finishes, so a follow up step - a validation, a formatter, a notification - has no other
    /// place to hook into. They run in order, and the first one that fails stops the rest.
    /// </summary>
    private bool RunAtSuccess()
    {
        if (this.environment.IsBeforeBuild)
        {
            // The before build pass reads the assembly of the previous build and generates a fraction of the
            // output. Running the commands here would run them on a stale result, and a second time afterwards.
            return true;
        }
        foreach (string command in this.environment.RunAtSuccess)
        {
            Logger.Trace($"Run at success: {command}");
            int exitCode = ShellProcess.Run(command, Environment.CurrentDirectory);
            if (exitCode != 0)
            {
                Logger.Error($"Run at success command '{command}' failed with exit code {exitCode}");
                return false;
            }
        }
        return true;
    }

    public static void InitializeLogger(string[] parameters)
    {
        // Move the log files from the entry assembly directory (KY.Core default) to the local application data
        // directory. The entry assembly directory is not writable in all scenarios, e.g. if the generator is installed
        // as a dotnet tool the directory is owned (and deleted on update) by the dotnet SDK. Has to happen before the
        // first log entry is written.
        if (FileSystem.IsAbsolute(GeneratorEnvironment.LocalApplicationDataPath))
        {
            Logger.File.Path = FileSystem.Combine(GeneratorEnvironment.LocalApplicationDataPath, "Logs");
        }
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
}
