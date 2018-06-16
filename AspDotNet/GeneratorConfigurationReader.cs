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
            XmlConverter.Map(x => x.Name("PreloadModule").ToList(nameof(GeneratorConfigurationController.PreloadModules)))
                        .Map(x => x.Name("Using").ToList(nameof(GeneratorConfigurationController.Usings)))
                        .Map(x => x.Name("Configure").ToList(nameof(GeneratorConfigurationController.Configures)).As(element => new GeneratorConfigurationConfigureModule(element.GetStringAttribute("Module"), element.Value)))
                        .Deserialize(configurationElement, configuration);
            return configuration;
        }
    }
}