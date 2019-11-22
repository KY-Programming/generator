using KY.Generator.Configurations;

namespace KY.Generator.Reflection.Configurations
{
    public class ReflectionReadConfiguration : ConfigurationBase
    {
        public string Assembly { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public bool SkipSelf { get; set; }
    }
}