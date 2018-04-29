using System.Xml.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Xml;
using KY.Generator.Configuration;

namespace KY.Generator.Reflection.Configuration
{
    internal class ReflectionConfigurationReader : ConfigurationReaderBase, IConfigurationReader
    {
        public string TagName => "Reflection";

        public ReflectionConfigurationReader(IDependencyResolver resolver)
            : base(resolver)
        { }

        public ConfigurationBase Read(XElement rootElement, XElement configurationElement)
        {
            ReflectionConfiguration configuration = new ReflectionConfiguration();
            this.ReadBase(rootElement, configuration);
            this.ReadBase(configurationElement, configuration);

            XmlConvert.Map<ReflectionType>("Type")
                      .MapList("Type", "Types")
                      .Deserialize(configurationElement, configuration);

            XElement defaultElement = configurationElement.Element("Default");
            if (defaultElement != null)
            {
                ReflectionType defaultType = XmlConvert.Deserialize<ReflectionType>(defaultElement);
                foreach (ReflectionType type in configuration.Types)
                {
                    type.SetDefaults(defaultType);
                    type.FieldsToProperties |= defaultType.FieldsToProperties;
                    type.PropertiesToFields |= defaultType.PropertiesToFields;
                    type.SkipNamespace |= defaultType.SkipNamespace;
                }
            }

            foreach (ReflectionType type in configuration.Types)
            {
                type.Configuration = configuration;
                if (string.IsNullOrEmpty(type.Name))
                {
                    throw new InvalidConfigurationException("Reflection type without Name is not allowed");
                }
                if (string.IsNullOrEmpty(type.Namespace))
                {
                    throw new InvalidConfigurationException($"Reflection type {type.Name} needs a Namespace");
                }
                if (type.Name.Contains("."))
                {
                    throw new InvalidConfigurationException($"Reflection type {type.Name} can not contain '.'. Please use the Namespace tag");
                }
            }
            return configuration;
        }
    }
}