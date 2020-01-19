using System.Collections.Generic;
using KY.Generator.Command;

namespace KY.Generator.Configuration
{
    public class ConfigurationEnvironment
    {
        public string ConfigurationFilePath { get; set; }
        public string OutputPath { get; set; }
        public List<CommandParameter> Parameters { get; }
        public string Command { get; set; }
        public string CommandGroup { get; set; }

        public ConfigurationEnvironment(string configurationFilePath = null, string outputPath = null)
        {
            this.ConfigurationFilePath = configurationFilePath;
            this.OutputPath = outputPath;
            this.Parameters = new List<CommandParameter>();
        }
    }
}