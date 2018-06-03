using System.Xml.Linq;
using KY.Core.Dependency;
using KY.Core.Xml;
using KY.Generator.Configuration;

namespace KY.Generator.Json.Configuration
{
    internal class JsonConfigurationReader : ConfigurationReaderBase, IConfigurationReader
    {
        public string TagName => "Json";

        public JsonConfigurationReader(IDependencyResolver resolver)
            : base(resolver)
        { }

        public ConfigurationBase Read(XElement rootElement, XElement configurationElement)
        {
            JsonConfiguration configuration = new JsonConfiguration();
            this.ReadBase(rootElement, configuration);
            this.ReadBase(configurationElement, configuration);

            XmlConverter.Deserialize(configurationElement, configuration);

            return configuration;
        }
    }
}