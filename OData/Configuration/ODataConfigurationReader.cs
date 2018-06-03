using System.Xml.Linq;
using KY.Core.Dependency;
using KY.Core.Xml;
using KY.Generator.Configuration;

namespace KY.Generator.OData.Configuration
{
    internal class ODataConfigurationReader : ConfigurationReaderBase, IConfigurationReader
    {
        public string TagName => "OData";

        public ODataConfigurationReader(IDependencyResolver resolver)
            : base(resolver)
        { }

        public ConfigurationBase Read(XElement rootElement, XElement configurationElement)
        {
            ODataConfiguration configuration = new ODataConfiguration();
            this.ReadBase(rootElement, configuration);
            this.ReadBase(configurationElement, configuration);
            XmlConverter.Deserialize(configurationElement, configuration);
            if (configuration.DataContext != null && configuration.DataContext.Name == null)
            {
                configuration.DataContext.Name = "DataContext";
            }
            //configuration.Connection = configurationElement.TryGetString(nameof(ODataConfiguration.Connection));
            //configuration.SkipNamespace = configurationElement.Exists(nameof(ODataConfiguration.SkipNamespace));
            //XElement dataContextElement = configurationElement.Element("DataContext");
            //if (dataContextElement != null)
            //{
            //    configuration.DataContext = new ODataEntityDataContext();
            //    configuration.DataContext.Name = dataContextElement.TryGetString("Name") ?? "DataContext";
            //    configuration.DataContext.RelativePath = dataContextElement.TryGetString("RelativePath");
            //}
            //XElement modelsElement = configurationElement.Element("Models");
            //if (modelsElement != null)
            //{
            //    configuration.Models = new ODataEntityModels();
            //    configuration.Models.RelativePath = modelsElement.TryGetString("RelativePath");
            //}
            return configuration;
        }
    }
}