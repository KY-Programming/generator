using System;
using KY.Generator.Configuration;

namespace KY.Generator.OData.Configuration
{
    internal class ODataGeneratorConfiguration
    {
        public IGenerator Generator { get; set; }
        public Type GeneratorType { get; set; }
        public IConfigurationReader ConfigurationReader { get; set; }
        public Type ConfigurationReaderType { get; set; }

        public ODataGeneratorConfiguration()
        {
            this.GeneratorType = typeof(ODataGenerator);
            this.ConfigurationReaderType = typeof(ODataConfigurationReader);
        }
    }
}