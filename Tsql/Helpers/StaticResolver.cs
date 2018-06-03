using KY.Generator.Mappings;
using KY.Generator.Tsql.Configuration;

namespace KY.Generator.Tsql
{
    internal static class StaticResolver
    {
        public static ITypeMapping TypeMapping { get; set; }
        public static TsqlGeneratorConfiguration GeneratorConfiguration { get; set; }
    }
}