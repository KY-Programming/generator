using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Mappings;
using KY.Generator.Module;
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
        private bool standalone;
        private CommandConfiguration command;
        private IList<ModuleBase> Modules { get; }

        public Generator()
        {
            Logger.CatchAll();
            Logger.Trace($"KY Generator v{Assembly.GetCallingAssembly().GetName().Version}");
            Logger.Trace("Current Directory: " + Environment.CurrentDirectory);
            Logger.Trace("Log Directory: " + Logger.File.Path);

            NugetPackageDependencyLoader.Activate();

            this.output = new FileOutput(AppDomain.CurrentDomain.BaseDirectory);
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();
            this.resolver.Bind<CommandRunner>().ToSelf();
            this.resolver.Bind<ModuleFinder>().ToSelf();
            this.resolver.Bind<IConfigurationReaderVersion>().To<ConfigurationReaderVersion2>();
            this.resolver.Bind<ReaderConfigurationMapping>().ToSingleton();
            this.resolver.Bind<WriterConfigurationMapping>().ToSingleton();
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
        }

        public static Generator Initialize()
        {
            return new Generator();
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
            this.resolver.Get<ReaderConfigurationMapping>().Map<TConfiguration, TReader>(name);
            return this;
        }

        public Generator RegisterWriter<TConfiguration, TWriter>(string name)
            where TConfiguration : ConfigurationBase
            where TWriter : ITransferWriter
        {
            this.resolver.Get<WriterConfigurationMapping>().Map<TConfiguration, TWriter>(name);
            return this;
        }

        public IGeneratorRunSyntax ReadConfiguration(string path)
        {
            Logger.Trace($"Read configuration from {path}");
            this.Modules.OfType<GeneratorModule>().ForEach(x => x.BeforeConfigure());
            this.command = new CommandConfiguration("run");
            this.command.Parameters.Add(new CommandValueParameter("path", path));
            this.command.Standalone = this.standalone;
            return this;
        }

        public IGeneratorRunSyntax ParseConfiguration(string configuration)
        {
            Logger.Trace($"Parse configuration {configuration}");
            this.Modules.OfType<GeneratorModule>().ForEach(x => x.BeforeConfigure());
            this.command = new CommandConfiguration("run");
            this.command.Parameters.Add(new CommandValueParameter("configuration", configuration));
            this.command.Standalone = this.standalone;
            return this;
        }

        public IGeneratorRunSyntax ParseCommand(params string[] arguments)
        {
            Logger.Trace($"Parse command {string.Join(" ", arguments)}");
            this.Modules.OfType<GeneratorModule>().ForEach(x => x.BeforeConfigure());
            CommandReader reader = this.resolver.Create<CommandReader>();
            this.command = reader.Read(arguments);
            this.command.Standalone = this.standalone;
            CommandValueParameter outputParameter = this.command.Parameters.OfType<CommandValueParameter>().FirstOrDefault(x => x.Name.Equals("output", StringComparison.CurrentCultureIgnoreCase));
            if (outputParameter != null)
            {
                this.SetOutput(outputParameter.Value);
            }
            return this;
        }

        public IGeneratorRunSyntax SetParameters(params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                Logger.Error("No parameters found. Provide at least a command or a path to a configuration file. Generation aborted!");
                return this;
            }

            if (FileSystem.FileExists(parameters.First()))
            {
                return this.SetOutput(parameters.Skip(1).FirstOrDefault())
                           .ReadConfiguration(parameters.First());
            }
            return this.ParseCommand(parameters);
        }

        public Generator SetStandalone()
        {
            this.standalone = true;
            if (this.command != null)
            {
                this.command.Standalone = true;
            }
            return this;
        }

        public bool Run()
        {
            try
            {
                return this.resolver.Get<CommandRunner>().Run(this.command, this.output);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
            finally
            {
                Logger.Trace("===============================");
            }
            return false;
        }
    }
}