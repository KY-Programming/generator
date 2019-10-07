using KY.Generator.Configuration;

namespace KY.Generator.Reflection.Configuration
{
    internal class ReflectionReadConfiguration : ConfigurationBase
    {
        public string Assembly { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
    }
}