using System;
using KY.Generator.OData.Configuration;

namespace KY.Generator.OData.Extensions
{
    public static class GeneratorExtension
    {
        public static Generator OData(this Generator generator, Action<IODataGeneratorConfigurationSyntax> action)
        {
            action(new ODataGeneratorConfigurationSyntax());
            return generator;
        }
    }
}