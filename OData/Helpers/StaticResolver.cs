using KY.Generator.Mappings;
using KY.Generator.OData.Configuration;

namespace KY.Generator.OData
{
    internal static class StaticResolver
    {
        public static ITypeMapping TypeMapping { get; set; }
        public static ODataGeneratorConfiguration GeneratorConfiguration { get; set; }
    }
}