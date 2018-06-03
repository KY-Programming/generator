using KY.Generator.Mappings;
using KY.Generator.Reflection.Configuration;

namespace KY.Generator.Reflection
{
    internal static class StaticResolver
    {
        public static ITypeMapping TypeMapping { get; set; }
        public static ReflectionGeneratorConfiguration GeneratorConfiguration { get; set; }
    }
}