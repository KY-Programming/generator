using System;
using System.Collections.Generic;
using KY.Generator.Configurations;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Transfers;
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
            if (model is JsonModelTransferObject && this.jsonConfiguration.WithReader)
            {
                this.WriteReader(classTemplate, model, configuration.FormatNames);
            }
            return classTemplate;
        }

        public IEnumerable<FileTemplate> Write(JsonWriteConfiguration configuration, List<ITransferObject> transferObjects)
        {
            this.jsonConfiguration = configuration;
            ModelWriteConfiguration modelWriteConfiguration = new ModelWriteConfiguration();
            modelWriteConfiguration.CopyBaseFrom(configuration);
            modelWriteConfiguration.Name = configuration.Name;
            modelWriteConfiguration.Namespace = configuration.Namespace;
            modelWriteConfiguration.SkipNamespace = configuration.SkipNamespace;
            modelWriteConfiguration.RelativePath = configuration.RelativePath;
            modelWriteConfiguration.FieldsToProperties = configuration.FieldsToProperties;
            modelWriteConfiguration.PropertiesToFields = configuration.PropertiesToFields;
            modelWriteConfiguration.FormatNames = configuration.FormatNames;
            return this.Write(modelWriteConfiguration, transferObjects);
        }

        private void WriteReader(ClassTemplate classTemplate, ModelTransferObject model, bool formatNames)
        {
            TypeTemplate objectType = Code.Type(model.Name, model.Namespace);
            if (model.Namespace != classTemplate.Namespace.Name && model.Namespace != null)
            {
                classTemplate.AddUsing(model.Namespace);
            }
            classTemplate.WithUsing("Newtonsoft.Json")
                         //.WithUsing("Newtonsoft.Json.Linq")
                         .WithUsing("System.IO");

            classTemplate.AddMethod("Load", objectType)
                         .FormatName(model.Language, formatNames)
                         .WithParameter(Code.Type("string"), "fileName")
                         .Static()
                         .Code.AddLine(Code.Return(Code.Method("Parse", Code.Local("File").Method("ReadAllText", Code.Local("fileName")))));

            classTemplate.AddMethod("Parse", objectType)
                         .FormatName(model.Language, formatNames)
                         .WithParameter(Code.Type("string"), "json")
                         .Static()
                         .Code.AddLine(Code.Return(Code.Local("JsonConvert").GenericMethod("DeserializeObject", objectType, Code.Local("json"))));
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