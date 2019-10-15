using System.Collections.Generic;
using System.Linq;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Json.Configuration;
using KY.Generator.Mappings;
using KY.Generator.Templates;
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
            if (this.jsonConfiguration.Object.WithReader)
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
            transferObjects.OfType<ModelTransferObject>().Single().Name = configuration.Object.Name;
            return this.Write(modelWriteConfiguration, transferObjects);
        }
    }
}