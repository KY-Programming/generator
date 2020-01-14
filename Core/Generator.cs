using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Mappings;
using KY.Generator.Output;
using KY.Generator.Syntax;
using KY.Generator.Transfer.Readers;
using KY.Generator.Transfer.Writers;

namespace KY.Generator
{
    public class Generator : IGeneratorRunSyntax, IGeneratorAfterRunSyntax
    {
        private IOutput output;
        private readonly DependencyResolver resolver;
        private readonly CommandConfiguration command;
        private bool success;
        private IList<ModuleBase> Modules { get; }

        public Generator(params string[] parameters)
        {
            InitializeLogger(parameters);
            Logger.Trace($"KY-Generator v{Assembly.GetCallingAssembly().GetName().Version}");
            Logger.Trace("Current Directory: " + Environment.CurrentDirectory);
            Logger.Trace("Log Directory: " + Logger.File.Path);
            Logger.Trace($"Parameters: {string.Join(" ", parameters)}");

            NugetPackageDependencyLoader.Activate();

            this.output = new FileOutput(AppDomain.CurrentDomain.BaseDirectory);
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.resolver.Bind<CommandRunner>().ToSelf();
            this.resolver.Bind<ModuleFinder>().ToSelf();
            this.resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.resolver.Bind<ConfigurationMapping>().ToSingleton();
            this.resolver.Bind<ConfigurationRunner>().ToSelf();
            this.resolver.Bind<ModelWriter>().ToSelf();
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

            CommandReader reader = this.resolver.Create<CommandReader>();
            this.command = reader.Read(parameters);
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

        public Generator SetStandalone()
        {
            this.command.Standalone = true;
            return this;
        }

        public IGeneratorAfterRunSyntax Run()
        {
            try
            {
                this.success = this.resolver.Get<CommandRunner>().Run(this.command, this.output);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                this.success = false;
            }
            finally
            {
                Logger.Trace("===============================");
            }
            if (!this.success && Logger.ErrorTargets.Contains(Logger.MsBuildOutput))
            {
                Logger.Error($"See the full log in: {Logger.File.Path}");
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