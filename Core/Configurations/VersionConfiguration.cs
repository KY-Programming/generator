using KY.Generator.Configuration;

namespace KY.Generator.Configurations
{
    public class VersionConfiguration : ConfigurationBase
    {
        [ConfigurationProperty("d")]
        public bool Detailed { get; set; }
    }
}