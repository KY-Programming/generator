using System.Xml.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Core.Xml;
using KY.Generator.Configuration;
using KY.Generator.Tsql.Entity;
using KY.Generator.Tsql.OData;

namespace KY.Generator.Tsql.Configuration
{
    public class TsqlConfigurationReader : ConfigurationReaderBase, IConfigurationReader
    {
        public string TagName => "Tsql";

        public TsqlConfigurationReader(IDependencyResolver resolver)
            : base(resolver)
        { }

        protected virtual TsqlConfiguration CreateConfiguration()
        {
            return new TsqlConfiguration();
        }

        public virtual ConfigurationBase Read(XElement rootElement, XElement configurationElement)
        {
            TsqlConfiguration configuration = this.CreateConfiguration();
            this.ReadBase(rootElement, configuration);
            this.ReadBase(configurationElement, configuration);

            XmlConverter.Map<TsqlEntity>("Entity")
                        .MapList("Entity", "Entities")
                        //.Map<TsqlDataContext>("DataContext")
                        //.MapList("DataContext", "Entities")
                        .Map("StoredProcedure",element => new TsqlStoredProcedure(element.Value))
                        .MapList("StoredProcedure", "StoredProcedures")
                        .Map(this.ReadControllerMethod)
                        .Deserialize(configurationElement, configuration);

            XElement defaultElement = configurationElement.Element("Default");
            if (defaultElement != null)
            {
                TsqlEntity defaultEntity = XmlConverter.Deserialize<TsqlEntity>(defaultElement);
                foreach (TsqlEntity entity in configuration.Entities)
                {
                    entity.SetDefaults(defaultEntity,
                                       entity.Controller == null ? nameof(entity.Controller) : null,
                                       entity.DataContext == null ? nameof(entity.DataContext) : null,
                                       entity.Enum == null ? nameof(entity.Enum) : null,
                                       entity.Model == null ? nameof(entity.Model) : null,
                                       entity.Repository == null ? nameof(entity.Repository) : null);
                }
            }

            foreach (TsqlEntity entity in configuration.Entities)
            {
                entity.Name = entity.Name ?? entity.Table ?? entity.StoredProcedure;
                entity.Configuration = configuration;
                if (entity.Model != null)
                {
                    entity.Model.Entity = entity;
                }
                if (entity.Controller != null)
                {
                    entity.Controller.Entity = entity;
                }
                if (entity.Repository != null)
                {
                    entity.Repository.Entity = entity;
                }
                if (entity.DataContext != null)
                {
                    entity.DataContext.Entity = entity;
                }
                if (entity.Enum != null)
                {
                    entity.Enum.Entity = entity;
                }
            }
            this.Validate(configuration);
            return configuration;
        }

        private TsqlODataControllerMethod ReadControllerMethod(XElement methodElement)
        {
            TsqlODataControllerMethod method = new TsqlODataControllerMethod(methodElement.Name.LocalName);
            foreach (XElement attributeElement in methodElement.Elements())
            {
                TsqlODataControllerMethodAttribute attribute = new TsqlODataControllerMethodAttribute(attributeElement.Name.LocalName);
                foreach (XElement attributePropertyElement in attributeElement.Elements())
                {
                    attribute.WithProperty(attributePropertyElement.Name.LocalName, attributePropertyElement.Value);
                }
                method.Attributes.Add(attribute);
            }
            return method;
        }

        private void Validate(TsqlConfiguration configuration)
        {
            foreach (TsqlEntity entity in configuration.Entities)
            {
                if (entity.Model != null || entity.Repository != null || entity.Controller != null)
                {
                    if (string.IsNullOrEmpty(entity.Schema))
                    {
                        throw new InvalidConfigurationException($"Tsql entity {nameof(entity.Schema)} can not be null or empty");
                    }
                    if (string.IsNullOrEmpty(entity.Table) && string.IsNullOrEmpty(entity.StoredProcedure))
                    {
                        throw new InvalidConfigurationException($"Tsql entity {nameof(entity.Table)} can not be null or empty");
                    }
                }
            }
        }
    }
}