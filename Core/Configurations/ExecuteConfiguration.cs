using KY.Generator.Configuration;

namespace KY.Generator.Configurations
{
    public class ExecuteConfiguration : ReadConfigurationBase
    {
        public string File { get; set; }

        [ConfigurationProperty("config")]
        public string Configuration { get; set; }
    }
}