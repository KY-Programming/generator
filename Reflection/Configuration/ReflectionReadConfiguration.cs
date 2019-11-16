using KY.Generator.Configuration;
using KY.Generator.Configurations;

namespace KY.Generator.Reflection.Configuration
{
    internal class ReflectionReadConfiguration : ConfigurationBase
    {
        public string Assembly { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
    }
}