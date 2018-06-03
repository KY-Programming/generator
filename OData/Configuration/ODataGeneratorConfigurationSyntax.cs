using KY.Generator.Configuration;

namespace KY.Generator.OData.Configuration
{
    public interface IODataGeneratorConfigurationSyntax
    {
        IODataGeneratorConfigurationSyntax SetGenerator(IGenerator generator);
        IODataGeneratorConfigurationSyntax SetGenerator<T>() where T : IGenerator;
        IODataGeneratorConfigurationSyntax SetConfigurationReader(IConfigurationReader reader);
        IODataGeneratorConfigurationSyntax SetConfigurationReader<T>() where T : IConfigurationReader;
    }

    public class ODataGeneratorConfigurationSyntax : IODataGeneratorConfigurationSyntax
    {
        public IODataGeneratorConfigurationSyntax SetGenerator(IGenerator generator)
        {
            StaticResolver.GeneratorConfiguration.Generator = generator;
            return this;
        }

        public IODataGeneratorConfigurationSyntax SetGenerator<T>() where T : IGenerator
        {
            StaticResolver.GeneratorConfiguration.GeneratorType = typeof(T);
            return this;
        }

        public IODataGeneratorConfigurationSyntax SetConfigurationReader(IConfigurationReader reader)
        {
            StaticResolver.GeneratorConfiguration.ConfigurationReader = reader;
            return this;
        }

        public IODataGeneratorConfigurationSyntax SetConfigurationReader<T>() where T : IConfigurationReader
        {
            StaticResolver.GeneratorConfiguration.ConfigurationReaderType = typeof(T);
            return this;
        }
    }
}