namespace KY.Generator.Configuration
{
    public class ConfigurationEnvironment
    {
        public string ConfigurationFilePath { get; }
        public string OutputPath { get; }

        public ConfigurationEnvironment(string configurationFilePath, string outputPath)
        {
            this.ConfigurationFilePath = configurationFilePath;
            this.OutputPath = outputPath;
        }
    }
}