using System;
using KY.Generator.Tsql.Configuration;

namespace KY.Generator.Tsql.Extensions
{
    public static class GeneratorExtension
    {
        public static Generator Tsql(this Generator generator, Action<ITsqlGeneratorConfigurationSyntax> action)
        {
            action(new TsqlGeneratorConfigurationSyntax());
            return generator;
        }
    }
}