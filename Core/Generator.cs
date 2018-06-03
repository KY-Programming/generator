using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Module;
using KY.Generator.Output;
using KY.Generator.Syntax;
using Newtonsoft.Json;

namespace KY.Generator
{
    public class Generator : IGeneratorRunSyntax
    {
        private IOutput output;
        private readonly DependencyResolver resolver;
        private List<ConfigurationBase> configrations;
        private IList<ModuleBase> Modules { get; set; }

        public Generator()
        {
            Logger.Trace($"KY Generator v{Assembly.GetCallingAssembly().GetName().Version}");
            Logger.Trace("CurrentDirectory: " + Environment.CurrentDirectory);
            this.output = new FileOutput(AppDomain.CurrentDomain.BaseDirectory);
            this.resolver = new DependencyResolver();
            this.resolver.Bind<ITypeMapping>().ToSingleton<TypeMapping>();

            Logger.Trace("Loaded modules:");
            ModuleFinder moduleFinder = new ModuleFinder(this.resolver);
            moduleFinder.LoadFromAssemblies();
            this.Modules = moduleFinder.Modules;
            this.Modules.ForEach(module => Logger.Trace($" - {module.GetType().Name.Trim("Module")}"));

            Logger.Trace("Initialize modules...");
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
            Logger.Trace("Output: " + this.output);
            return this;
        }

        public Generator SetOutput(string path)
        {
            return this.SetOutput(new FileOutput(path));
        }

        public IGeneratorRunSyntax ReadConfiguration(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Logger.Error("Invalid parameters: Provide at least path to config file)");
                return this;
            }
            Logger.Trace("Settings: " + path);
            return this.ParseConfiguration(FileSystem.ReadXml(path));
        }

        public IGeneratorRunSyntax ParseConfiguration(string configuration)
        {
            Logger.Trace("Settings: " + "Memory");
            if (configuration.TrimStart().StartsWith("<"))
            {
                return this.ParseConfiguration(XElement.Parse(configuration));
            }
            configuration = $"{{ \"Configuration\": {configuration} }}";
            return this.ParseConfiguration(JsonConvert.DeserializeXNode(configuration).Root);
        }

        public IGeneratorRunSyntax ParseConfiguration(XElement element)
        {
            this.Modules.OfType<GeneratorModule>().ForEach(x => x.BeforeConfigure());
            ConfigurationsReader configurationsReader = this.resolver.Create<ConfigurationsReader>();
            this.configrations = configurationsReader.Read(element).ToList();
            return this;
        }

        public bool Run()
        {
            try
            {
                if (this.configrations == null)
                {
                    Logger.Trace("No configuration loaded. Generation failed!");
                    return false;
                }
                this.Modules.OfType<GeneratorModule>().ForEach(x => x.BeforeRun());
                List<IGenerator> generators = this.resolver.Get<List<IGenerator>>();
                Logger.Trace($"Start generating {this.configrations.Count} configurations");
                if (!(this.configrations.FirstOrDefault()?.VerifySsl ?? true))
                {
                    ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                }
                bool success = true;
                foreach (ConfigurationBase configuration in this.configrations)
                {
                    if (configuration.Language == null)
                    {
                        Logger.Trace("Configuration without language found. Generation failed!");
                        success = false;
                        continue;
                    }
                    try
                    {
                        foreach (IGenerator generator in generators)
                        {
                            generator.Generate(configuration);
                            generator.Files?.ForEach(x => configuration.Language.Write(x, this.output));
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Error(exception);
                    }
                }
                this.Modules.OfType<GeneratorModule>().ForEach(x => x.AfterRun());
                Logger.Trace("All configurations generated");
                return success;
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