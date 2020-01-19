using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public class Generator : IGeneratorRunSyntax, IGeneratorAfterRunSyntax
    {
        private readonly DependencyResolver resolver;
        private readonly List<string> parameters;
        private IOutput output;
        private bool success;
        private IList<ModuleBase> Modules { get; }

        public Generator(params string[] parameters)
        {
            this.parameters = parameters.ToList();
            InitializeLogger(parameters);
            Logger.Trace($"KY-Generator v{Assembly.GetCallingAssembly().GetName().Version}");
            Logger.Trace("Current Directory: " + Environment.CurrentDirectory);
            Logger.Trace("Log Directory: " + Logger.File.Path);
            Logger.Trace($"Parameters: {string.Join(" ", parameters)}");

            NugetPackageDependencyLoader.Activate();

            this.output = new FileOutput(AppDomain.CurrentDomain.BaseDirectory);
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.resolver.Bind<CommandReader>().ToSelf();
            this.resolver.Bind<CommandRegistry>().ToSingleton();
            this.resolver.Bind<ModuleFinder>().ToSelf();
            this.resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.resolver.Bind<ConfigurationRunner>().ToSelf();
            StaticResolver.Resolver = this.resolver;

            ModuleFinder moduleFinder = this.resolver.Get<ModuleFinder>();
            moduleFinder.LoadFromAssemblies();
            this.Modules = moduleFinder.Modules;
            foreach (ModuleBase module in this.Modules)
            {
                Logger.Trace($"{module.GetType().Name.Replace("Module", "")}-{module.GetType().Assembly.GetName().Version} module loaded");
            }
            this.Modules.ForEach(module => this.resolver.Bind<ModuleBase>().To(module));
            this.Modules.ForEach(module => module.Initialize());
        }

        public static Generator Create(params string[] parameters)
        {
            return new Generator(parameters);
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

        public Generator RegisterCommand<TCommand, TConfiguration>(string name, string group = CommandRegistry.DefaultGroup)
            where TCommand : ICommand
            where TConfiguration : IConfiguration
        {
            this.resolver.Get<CommandRegistry>().Register<TCommand, TConfiguration>(name, group);
            return this;
        }

        public Generator SetStandalone()
        {
            this.parameters.Add("-standalone");
            return this;
        }

        public IGeneratorAfterRunSyntax Run()
        {
            try
            {
                IConfiguration configuration = this.resolver.Get<CommandReader>().Read(this.parameters);
                if (configuration == null)
                {
                    this.success = false;
                }
                else
                {
                    configuration.Output = this.output;
                    ICommand command = this.resolver.Get<CommandRegistry>().CreateCommand(configuration);
                    if (command is ICommandLineCommand commandLineCommand)
                    {
                        this.success = commandLineCommand.Execute(configuration);
                        if (this.success)
                        {
                            configuration.Output?.Execute();
                        }
                    }
                    else
                    {
                        Logger.Error($"Can not execute command '{command.GetType().Name.TrimEnd("Command")}'. Command can only be used in configuration file.");
                        this.success = false;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                this.success = false;
            }
            finally
            {
                if (!this.success && Logger.ErrorTargets.Contains(Logger.MsBuildOutput))
                {
                    Logger.Error($"See the full log in: {Logger.File.Path}");
                }
                Logger.Trace("===============================");
            }
            return this;
        }

        public bool GetResult()
        {
            return this.success;
        }

        public void SetExitCode()
        {
            if (!this.success)
            {
                Environment.ExitCode = 1;
            }
        }

        private static void InitializeLogger(string[] parameters)
        {
            Logger.CatchAll();
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
            if (parameters.Any(parameter => "msbuild".Equals(parameter)))
            {
                Logger.Trace("MsBuild trace mode activated");
                Logger.WarningTargets.Add(Logger.MsBuildOutput);
                Logger.ErrorTargets.Add(Logger.MsBuildOutput);
                Logger.WarningTargets.Remove(Logger.VisualStudioOutput);
                Logger.ErrorTargets.Remove(Logger.VisualStudioOutput);
            }
        }
    }
}