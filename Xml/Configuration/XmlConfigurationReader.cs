using System.Xml.Linq;
using KY.Core.Dependency;
using KY.Core.Xml;
using KY.Generator.Configuration;

namespace KY.Generator.Xml.Configuration
{
    public class XmlConfigurationReader : ConfigurationReaderBase, IConfigurationReader
    {
        public string TagName => "Xml";

        public XmlConfigurationReader(IDependencyResolver resolver)
            : base(resolver)
        { }

        public ConfigurationBase Read(XElement rootElement, XElement configurationElement)
        {
            XmlConfiguration configuration = new XmlConfiguration();
            this.ReadBase(rootElement, configuration);
            this.ReadBase(configurationElement, configuration);

            XmlConverter.Deserialize(configurationElement, configuration);

            return configuration;
        }
    }
}