using System.Collections.Generic;
using KY.Generator.Command;

namespace KY.Generator.Configuration
{
    public class ConfigurationEnvironment
    {
        public string ConfigurationFilePath { get; }
        public string OutputPath { get; }
        public List<ICommandParameter> Parameters { get; set; }

        public ConfigurationEnvironment(string configurationFilePath, string outputPath)
        {
            this.ConfigurationFilePath = configurationFilePath;
            this.OutputPath = outputPath;
        }
    }
}