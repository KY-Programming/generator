using System.Collections.Generic;
using System.Linq;
using System.Net;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Models;
using KY.Generator.Module;
using KY.Generator.Output;

namespace KY.Generator.Commands
{
    internal class RunCommand : IGeneratorCommand
    {
        private readonly List<ModuleBase> modules;
        private readonly IDependencyResolver resolver;
        private readonly ConfigurationRunner runner;

        public string[] Names { get; } = { "run" };

        public RunCommand(List<ModuleBase> modules, IDependencyResolver resolver, ConfigurationRunner runner)
        {
            this.modules = modules;
            this.resolver = resolver;
            this.runner = runner;
        }

        public bool Generate(CommandConfiguration configuration, ref IOutput output)
        {
            Logger.Trace("Execute run command...");
            string outputPath = configuration.Parameters.GetString("output");
            if (!string.IsNullOrEmpty(outputPath))
            {
                output = new FileOutput(outputPath);
            }
            if (output == null)
            {
                Logger.Error("No valid output specified");
                return false;
            }
            Logger.Trace("Output: " + output);
            string path = configuration.Parameters.GetString("path");
            if (!string.IsNullOrEmpty(path))
            {
                Logger.Trace("Read settings from: " + path);
                return this.Run(FileSystem.ReadAllText(path), path, configuration, output);
            }
            string serialized = configuration.Parameters.GetString("configuration");
            if (!string.IsNullOrEmpty(serialized))
            {
                Logger.Trace("Read settings from: Memory");
                return this.Run(serialized, configuration, output);
            }
            Logger.Error("Invalid parameters: Provide at least path to config file)");
            return false;
        }

        public bool Run(string serializedConfiguration, CommandConfiguration configuration, IOutput output)
        {
            List<ConfigurationSet> configurations = this.Deserialize(serializedConfiguration, output);
            return this.Run(configurations, configuration, output);
        }

        private bool Run(string serializedConfiguration, string path, CommandConfiguration configuration, IOutput output)
        {
            List<ConfigurationSet> configurations = this.Deserialize(serializedConfiguration, output);
            ConfigurationEnvironment environment = new ConfigurationEnvironment(path, output.ToString());
            configurations.SelectMany(x => x.Configurations)
                          .ForEach(x => x.Environment = environment);
            return this.Run(configurations, configuration, output);
        }

        private List<ConfigurationSet> Deserialize(string configuration, IOutput output)
        {
            ConfigurationsReader configurationsReader = this.resolver.Create<ConfigurationsReader>();
            return configurationsReader.Parse(configuration, output);
        }

        private bool Run(List<ConfigurationSet> configurations, CommandConfiguration configuration, IOutput output)
        {
            configuration.AssertIsNotNull(nameof(configurations), "No configuration loaded. Generation failed!");
            bool isBeforeBuild = configuration.Parameters.Exists("beforeBuild");
            if (isBeforeBuild)
            {
                Logger.Trace("Run only configurations flagged with \"beforeBuild\": true");
            }
            if (configurations == null || configurations.Count == 0)
            {
                Logger.Trace("No configuration loaded. Provide at least one entry like: ...\"generate\": [{\"write\": \"demo\"}]...");
                return false;
            }
            this.modules.OfType<GeneratorModule>().ForEach(x => x.BeforeRun());
            if (configurations.Any(x => x.Configurations.Any(y => !y.VerifySsl)))
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
            bool success = this.runner.Run(configurations, output, isBeforeBuild);
            this.modules.OfType<GeneratorModule>().ForEach(x => x.AfterRun());
            Logger.Trace("All configurations generated");
            return success;
        }
    }
}