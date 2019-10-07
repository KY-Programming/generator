using System;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Configuration;

namespace KY.Generator.Reflection
{
    //TODO: Drop
    [Obsolete("Do not use this again")]
    internal static class StaticResolver
    {
        public static ITypeMapping TypeMapping { get; set; }
        //public static ReflectionGeneratorConfiguration GeneratorConfiguration { get; set; }
    }
}