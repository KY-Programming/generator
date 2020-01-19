using KY.Generator.Configuration;

namespace KY.Generator.Reflection.Configurations
{
    public class ReadReflectionConfiguration : ConfigurationBase
    {
        public string Assembly { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool SkipSelf { get; set; }
    }
}