using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Output;

namespace KY.Generator.Commands
{
    internal class ExecuteCommand : ICommandLineCommand
    {
        private readonly ConfigurationsReader reader;
        private readonly ConfigurationRunner runner;

        public ExecuteCommand(ConfigurationsReader reader, ConfigurationRunner runner)
        {
            this.reader = reader;
            this.runner = runner;
        }

        public bool Execute(IConfiguration configurationBase)
        {
            ExecuteConfiguration configuration = (ExecuteConfiguration)configurationBase;
            string serializedConfiguration;
            if (!string.IsNullOrEmpty(configuration.Configuration))
            {
                serializedConfiguration = configuration.Configuration;
            }
            else
            {
                if (string.IsNullOrEmpty(configuration.File))
                {
                    Logger.Error("Execute command without -file parameter found");
                    return false;
                }
                if (!FileSystem.FileExists(configuration.File))
                {
                    Logger.Error($"Execute command can not find file \"{configuration.File}\". Searched in \"{FileSystem.Parent(FileSystem.ToAbsolutePath(configuration.File))}\"");
                    return false;
                }
                if (configuration.Output == null)
                {
                    configuration.Output = new FileOutput(FileSystem.Parent(configuration.File));
                    Logger.Trace($"Output: {configuration.Output}");
                }
                serializedConfiguration = FileSystem.ReadAllText(configuration.File);
            }
            Logger.Trace("Read configurations...");
            ExecuteConfiguration executeConfiguration = this.reader.Parse(serializedConfiguration);
            executeConfiguration.Execute.Flatten().ForEach(x => x.Output = configuration.Output);
            return this.runner.Run(executeConfiguration.Yield());
        }

        //private bool Run(string serializedConfiguration, string path, CommandConfiguration configuration, IOutput output)
        //{
        //    List<ConfigurationSet> configurations = this.Deserialize(serializedConfiguration, output);
        //    ConfigurationEnvironment environment = new ConfigurationEnvironment(path, output.ToString());
        //    configurations.SelectMany(x => x.Configurations)
        //                  .ForEach(x => x.Environment = environment);
        //    return this.Run(configurations, configuration, output);
        //}

        //private bool Run(List<ConfigurationSet> configurations, CommandConfiguration configuration, IOutput output)
        //{
        //    configuration.AssertIsNotNull(nameof(configurations), "No configuration loaded. Generation failed!");
        //    bool isBeforeBuild = configuration.Parameters.Exists("beforeBuild");
        //    if (isBeforeBuild)
        //    {
        //        Logger.Trace("Run only configurations flagged with \"beforeBuild\": true");
        //    }
        //    if (configurations == null || configurations.Count == 0)
        //    {
        //        Logger.Trace("No configuration loaded. Provide at least one entry like: ...\"generate\": [{\"write\": \"demo\"}]...");
        //        return false;
        //    }
        //    if (configurations.Any(x => x.Configurations.Any(y => !y.VerifySsl)))
        //    {
        //        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        //    }
        //    bool success = this.runner.Run(configurations, output, isBeforeBuild);
        //    Logger.Trace("All configurations generated");
        //    return success;
        //}
    }
}