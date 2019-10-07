using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Core.Module;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Module;
using KY.Generator.Output;
using KY.Generator.Transfer;

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

        public bool Generate(CommandConfiguration configuration, IOutput output)
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
                return this.Run(FileSystem.ReadAllText(path), path, output);
            }
            string serialized = configuration.Parameters.GetString("configuration");
            if (!string.IsNullOrEmpty(serialized))
            {
                Logger.Trace("Read settings from: Memory");
                return this.Run(serialized, output);
            }
            Logger.Error("Invalid parameters: Provide at least path to config file)");
            return false;
        }

        public bool Run(string configuration, IOutput output)
        {
            List<ConfigurationPair> configurations = this.Deserialize(configuration);
            return this.Run(configurations, output);
        }

        private bool Run(string configuration, string path, IOutput output)
        {
            List<ConfigurationPair> configurations = this.Deserialize(configuration);
            configurations.SelectMany(x => x.Readers).ForEach(x => x.ConfigurationFilePath = path);
            configurations.SelectMany(x => x.Writers).ForEach(x => x.ConfigurationFilePath = path);
            return this.Run(configurations, output);
        }

        private List<ConfigurationPair> Deserialize(string configuration)
        {
            ConfigurationsReader configurationsReader = this.resolver.Create<ConfigurationsReader>();
            return configurationsReader.Parse(configuration);
        }

        private bool Run(List<ConfigurationPair> configurations, IOutput output)
        {
            if (configurations == null || configurations.Count == 0)
            {
                Logger.Trace("No configuration loaded. Generation failed!");
                return false;
            }
            this.modules.OfType<GeneratorModule>().ForEach(x => x.BeforeRun());
            if (configurations.Any(x => x.Readers.Any(y => !y.VerifySsl) || x.Writers.Any(y => !y.VerifySsl)))
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
            bool success = this.runner.Run(configurations, output);
            this.modules.OfType<GeneratorModule>().ForEach(x => x.AfterRun());
            Logger.Trace("All configurations generated");
            return success;
        }
    }
}