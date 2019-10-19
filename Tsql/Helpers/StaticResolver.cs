using System;
using KY.Generator.Mappings;

namespace KY.Generator.Tsql
{
    [Obsolete("Do not use again!")]
    internal static class StaticResolver
    {
        public static ITypeMapping TypeMapping { get; set; }
    }
}