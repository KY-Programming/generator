using System.Xml.Linq;
using KY.Core.Dependency;
using KY.Core.Xml;
using KY.Generator.Configuration;

namespace KY.Generator.AspDotNet
{
    internal class GeneratorConfigurationReader : ConfigurationReaderBase, IConfigurationReader
    {
        public string TagName => "Generator";

        public GeneratorConfigurationReader(IDependencyResolver resolver)
            : base(resolver)
        { }

        public ConfigurationBase Read(XElement rootElement, XElement configurationElement)
        {
            GeneratorConfiguration configuration = new GeneratorConfiguration();
            this.ReadBase(rootElement, configuration);
            this.ReadBase(configurationElement, configuration);
            XmlConvert.Deserialize(configurationElement, configuration);
            return configuration;
        }
    }
}