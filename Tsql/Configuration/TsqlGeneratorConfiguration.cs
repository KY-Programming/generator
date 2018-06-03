using KY.Generator.Configuration;

namespace KY.Generator.Tsql.Configuration
{
    internal class TsqlGeneratorConfiguration
    {
        public IGenerator Generator { get; set; }
        public System.Type GeneratorType { get; set; }
        public IConfigurationReader ConfigurationReader { get; set; }
        public System.Type ConfigurationReaderType { get; set; }

        public TsqlGeneratorConfiguration()
        {
            this.GeneratorType = typeof(TsqlGenerator);
            this.ConfigurationReaderType = typeof(TsqlConfigurationReader);
        }
    }
}