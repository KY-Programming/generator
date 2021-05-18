using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Syntax;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;
using KY.Generator.Transfer.Writers;

namespace KY.Generator
{
    public class Generator : IGeneratorRunSyntax
    {
        private IOutput output;
        private readonly DependencyResolver resolver;
        private readonly List<IGeneratorCommand> commands = new List<IGeneratorCommand>();
        private readonly List<ITransferObject> transferObjects = new List<ITransferObject>();

        public Generator()
        {
            Logger.CatchAll();
            Assembly callingAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            FrameworkName framework = callingAssembly.GetTargetFramework();
            Logger.Trace($"KY-Generator v{callingAssembly.GetName().Version} ({framework.Identifier.Replace("App", string.Empty)} {framework.Version.Major}.{framework.Version.Minor})");
            Logger.Trace("Current Directory: " + Environment.CurrentDirectory);
            Logger.Trace("Log Directory: " + Logger.File.Path);

            NugetPackageDependencyLoader.Activate();
            NugetPackageDependencyLoader.ResolveDependencies(this.GetType().Assembly);

            this.output = new FileOutput(Environment.CurrentDirectory);
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.resolver.Bind<CommandRunner>().ToSelf();
            this.resolver.Bind<ModuleFinder>().ToSingleton();
            this.resolver.Bind<ConfigurationMapping>().ToSingleton();
            this.resolver.Bind<ModelWriter>().ToSelf();

            ModuleFinder moduleFinder = this.resolver.Get<ModuleFinder>();
            this.InitializeModules(moduleFinder.Modules);
        }

        public static Generator Initialize()
        {
            return new Generator();
        }

        public Generator PreloadModules(string path, string moduleFileNameSearchPattern = default)
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

        public Generator SetOutput(IOutput newOutput)
        {
            this.output = newOutput;
            return this;
        }

        public Generator SetOutput(string path)
        {
            return this.SetOutput(new FileOutput(path));
        }

        public Generator RegisterCommand<T>() where T : IGeneratorCommand
        {
            this.resolver.Bind<IGeneratorCommand>().To<T>();
            return this;
        }

        public Generator RegisterCommand(IGeneratorCommand generator)
        {
            this.resolver.Bind<IGeneratorCommand>().To(generator);
            return this;
        }

        public Generator RegisterReader<TConfiguration, TReader>(string name)
            where TConfiguration : ConfigurationBase
            where TReader : ITransferReader
        {
            this.resolver.Get<ConfigurationMapping>().Map<TConfiguration, TReader>(name);
            return this;
        }

        public Generator RegisterWriter<TConfiguration, TWriter>(string name)
            where TConfiguration : ConfigurationBase
            where TWriter : ITransferWriter
        {
            this.resolver.Get<ConfigurationMapping>().Map<TConfiguration, TWriter>(name);
            return this;
        }

        public IGeneratorRunSyntax ParseAttributes(string assemblyName)
        {
            Logger.Trace($"Read attributes from assembly {assemblyName}");
            IGeneratorCommand command = this.resolver.Get<CommandRunner>().FindCommand("RunByAttributes");
            command.Parse(
                new RawCommandParameter("assembly", assemblyName),
                new RawCommandParameter("SkipAsyncCheck", bool.TrueString)
            );
            command.TransferObjects = this.transferObjects;
            this.commands.Add(command);
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

            List<RawCommand> rawCommands = RawCommandReader.Read(parameters);
            this.commands.AddRange(this.resolver.Get<CommandRunner>().Convert(rawCommands, this.transferObjects));
            return this;
        }

        public bool Run()
        {
            bool success = true;
            try
            {
                List<ILanguage> languages = this.resolver.Get<List<ILanguage>>();
                GeneratorCommand.AddParser(value => languages.FirstOrDefault(x => x.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase)));
                CommandRunner runner = this.resolver.Get<CommandRunner>();
                List<IGeneratorCommand> asyncCommands = new List<IGeneratorCommand>();
                IGeneratorCommandResult switchContext = null;
                bool switchAsync = false;
                foreach (IGeneratorCommand command in this.commands)
                {
                    IGeneratorCommandResult result = runner.Run(command, this.output);
                    success &= result.Success;
                    if (result.SwitchContext)
                    {
                        switchContext = switchContext ?? result;
                        asyncCommands.Add(command);
                    }
                    if (result.SwitchToAsync)
                    {
                        switchAsync = true;
                        asyncCommands.Add(command);
                    }
                    if (result.RerunOnAsync)
                    {
                        asyncCommands.Add(command);
                    }
                }
                if (switchAsync)
                {
                    return this.SwitchToAsync(asyncCommands);
                }
                if (switchContext != null)
                {
                    return this.SwitchContext(switchContext, asyncCommands);
                }
                if (success)
                {
                    this.output.Execute();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                success = false;
            }
            finally
            {
                Logger.Trace("===============================");
            }
            if (!success && Logger.ErrorTargets.Contains(Logger.MsBuildOutput))
            {
                Logger.Error($"See the full log in: {Logger.File.Path}");
            }
            return success;
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
            Regex regex = new Regex(@"(?<separator>[\\/])(?<framework>net[^\\/]+)[\\/]");
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
                ProcessStartInfo startInfo = new ProcessStartInfo();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && FileSystem.FileExists(locationExe))
                {
                    // Always use the .exe on Windows to fix the dotnet.exe x86 problem
                    startInfo.FileName = locationExe;
                }
                else
                {
                    startInfo.FileName = "dotnet";
                    startInfo.Arguments = location;
                }
                startInfo.Arguments += string.Join(" ", commandsToRun);
                if (result.SwitchToArchitecture != null)
                {
                    startInfo.Arguments += $" --switchedFromArchitecture=\"{result.SwitchToArchitecture}\"";
                }
                if (result.SwitchToFramework != SwitchableFramework.None)
                {
                    startInfo.Arguments += $" --switchedFromFramework=\"{result.SwitchToFramework}\"";
                }
                //startInfo.UseShellExecute = false;
                //startInfo.RedirectStandardOutput = true;
                //startInfo.RedirectStandardError = true;
                Logger.Trace("===============================");
                Process process = Process.Start(startInfo);
                process.OutputDataReceived += (sender, args) => Logger.Trace(">> " + args.Data);
                process.ErrorDataReceived += (sender, args) => Logger.Error(">> " + args.Data);
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
            string location = Assembly.GetEntryAssembly()?.Location ?? throw new InvalidOperationException("No location found");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && location.EndsWith(".exe"))
            {
                startInfo.FileName = location;
            }
            else
            {
                startInfo.FileName = "dotnet";
                startInfo.Arguments = location;
            }
            startInfo.Arguments += " " + string.Join(" ", commandsToRun);
            startInfo.Arguments += " --only-async";
            Process.Start(startInfo);
            return true;
        }

        public static void InitializeLogger(string[] parameters)
        {
            Logger.Console.ShortenEntries = false;
            Logger.AllTargets.Add(Logger.VisualStudioOutput);
            if (parameters.Any(parameter => parameter.ToLowerInvariant().Contains("forwardlogging")))
            {
                ForwardConsoleTarget target = new ForwardConsoleTarget();
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
            list.ForEach(module => this.resolver.Bind<ModuleBase>().To(module));
            list.ForEach(module => module.Initialize());
            list.ForEach(module => Logger.Trace($"{module.GetType().Name.Replace("Module", "")}-{module.GetType().Assembly.GetName().Version} module loaded"));
        }
    }
}