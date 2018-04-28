using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using KY.Core.Dependency;
using KY.Generator.Languages;
using KY.Generator.Mappings;

namespace KY.Generator.Configuration
{
    public abstract class ConfigurationReaderBase
    {
        private readonly List<ILanguage> languages;
        private readonly ClassMappingReader classMappingReader;
        private readonly FieldMappingReader fieldMappingReader;
        private readonly PropertyMappingReader propertyMappingReader;

        protected ConfigurationReaderBase(IDependencyResolver resolver)
        {
            this.languages = resolver.Get<List<ILanguage>>();
            this.classMappingReader = resolver.Create<ClassMappingReader>();
            this.fieldMappingReader = resolver.Create<FieldMappingReader>();
            this.propertyMappingReader = resolver.Create<PropertyMappingReader>();
        }

        protected void ReadBase(XElement configurationElement, ConfigurationBase configuration)
        {
            XElement addHeaderElement = configurationElement.Element("AddHeader");
            if (addHeaderElement != null)
            {
                configuration.AddHeader = bool.TrueString.Equals(addHeaderElement.Value) || string.IsNullOrEmpty(addHeaderElement.Value);
            }
            XElement verifySslElement = configurationElement.Element("VerifySSL");
            if (verifySslElement != null)
            {
                configuration.VerifySsl = bool.TrueString.Equals(verifySslElement.Value) || string.IsNullOrEmpty(verifySslElement.Value);
            }
            XElement languageElement = configurationElement.Element("Language");
            if (languageElement != null)
            {
                configuration.Language = this.languages.FirstOrDefault(x => x.Name.Equals(languageElement.Value, StringComparison.InvariantCultureIgnoreCase));
            }
            configuration.ClassMapping.AddRange(this.classMappingReader.Read(configurationElement));
            configuration.FieldMapping.AddRange(this.fieldMappingReader.Read(configurationElement));
            configuration.PropertyMapping.AddRange(this.propertyMappingReader.Read(configurationElement));
        }
    }
}