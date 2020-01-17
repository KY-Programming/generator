using System.Collections.Generic;
using KY.Generator.Command;

namespace KY.Generator.Configuration
{
    public class ConfigurationEnvironment
    {
        public string ConfigurationFilePath { get; }
        public string OutputPath { get; }
        public List<CommandParameter> Parameters { get; }

        public ConfigurationEnvironment(string configurationFilePath, string outputPath)
        {
            this.ConfigurationFilePath = configurationFilePath;
            this.OutputPath = outputPath;
            this.Parameters = new List<CommandParameter>();
        }
    }
}