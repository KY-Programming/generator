using KY.Generator.Configuration;

namespace KY.Generator.Tsql.Configuration
{
    public interface ITsqlGeneratorConfigurationSyntax
    {
        ITsqlGeneratorConfigurationSyntax SetGenerator(IGenerator generator);
        ITsqlGeneratorConfigurationSyntax SetGenerator<T>() where T : IGenerator;
        ITsqlGeneratorConfigurationSyntax SetConfigurationReader(IConfigurationReader reader);
        ITsqlGeneratorConfigurationSyntax SetConfigurationReader<T>() where T : IConfigurationReader;
    }

    public class TsqlGeneratorConfigurationSyntax : ITsqlGeneratorConfigurationSyntax
    {
        public ITsqlGeneratorConfigurationSyntax SetGenerator(IGenerator generator)
        {
            StaticResolver.GeneratorConfiguration.Generator = generator;
            return this;
        }

        public ITsqlGeneratorConfigurationSyntax SetGenerator<T>() where T : IGenerator
        {
            StaticResolver.GeneratorConfiguration.GeneratorType = typeof(T);
            return this;
        }

        public ITsqlGeneratorConfigurationSyntax SetConfigurationReader(IConfigurationReader reader)
        {
            StaticResolver.GeneratorConfiguration.ConfigurationReader = reader;
            return this;
        }

        public ITsqlGeneratorConfigurationSyntax SetConfigurationReader<T>() where T : IConfigurationReader
        {
            StaticResolver.GeneratorConfiguration.ConfigurationReaderType = typeof(T);
            return this;
        }
    }
}