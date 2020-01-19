using System;
using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.TransferObjects;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;

namespace KY.Generator.Json.Writers
{
    internal class ObjectWriter : ModelWriter
    {
        private JsonWriteConfiguration jsonConfiguration;

        public ObjectWriter(ITypeMapping typeMapping)
            : base(typeMapping)
        { }

        protected override ClassTemplate WriteClass(IModelConfiguration configuration, ModelTransferObject model, string nameSpace, List<FileTemplate> files)
        {
            ClassTemplate classTemplate = base.WriteClass(configuration, model, nameSpace, files);
            if (model is JsonModelTransferObject && this.jsonConfiguration.Object.WithReader)
            {
                ObjectReaderWriter.WriteReader(classTemplate, model, configuration.FormatNames);
            }
            return classTemplate;
        }

        public IEnumerable<FileTemplate> Write(JsonWriteConfiguration configuration, List<ITransferObject> transferObjects)
        {
            this.jsonConfiguration = configuration;
            ModelWriteConfiguration modelWriteConfiguration = new ModelWriteConfiguration();
            modelWriteConfiguration.CopyBaseFrom(configuration);
            modelWriteConfiguration.Name = configuration.Object.Name;
            modelWriteConfiguration.Namespace = configuration.Object.Namespace;
            modelWriteConfiguration.SkipNamespace = configuration.Object.SkipNamespace;
            modelWriteConfiguration.RelativePath = configuration.Object.RelativePath;
            modelWriteConfiguration.FieldsToProperties = configuration.Object.FieldsToProperties;
            modelWriteConfiguration.PropertiesToFields = configuration.Object.PropertiesToFields;
            modelWriteConfiguration.FormatNames = configuration.Object.FormatNames;
            return this.Write(modelWriteConfiguration, transferObjects);
        }

        protected override FieldTemplate AddField(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IConfiguration configuration)
        {
            FieldTemplate fieldTemplate = base.AddField(model, name, type, classTemplate, configuration);
            if (!fieldTemplate.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                fieldTemplate.WithAttribute("JsonProperty", Code.String(name));
                classTemplate.AddUsing("Newtonsoft.Json");
            }
            return fieldTemplate;
        }

        protected override PropertyTemplate AddProperty(ModelTransferObject model, string name, TypeTransferObject type, ClassTemplate classTemplate, IConfiguration configuration, bool canRead = true, bool canWrite = true)
        {
            PropertyTemplate propertyTemplate = base.AddProperty(model, name, type, classTemplate, configuration, canRead, canWrite);
            if (!propertyTemplate.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                propertyTemplate.WithAttribute("JsonProperty", Code.String(name));
                classTemplate.AddUsing("Newtonsoft.Json");
            }
            return propertyTemplate;
        }
    }
}