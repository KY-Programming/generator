using System.Collections.Generic;
using KY.Core.DataAccess;
using KY.Generator.Configuration;
using KY.Generator.Json.Configuration;
using KY.Generator.Json.Writers;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Json
{
    public class JsonGenerator : IGenerator
    {
        private readonly ITypeMapping typeMapping;
        public List<FileTemplate> Files { get; }

        public JsonGenerator(ITypeMapping typeMapping)
        {
            this.typeMapping = typeMapping;
            this.Files = new List<FileTemplate>();
        }

        public void Generate(ConfigurationBase configurationBase)
        {
            this.Files.Clear();
            JsonConfiguration configuration = configurationBase as JsonConfiguration;
            if (configuration == null)
            {
                return;
            }
            if (configuration.ToObject != null)
            {
                new ObjectWriter(this, this.typeMapping).Write(configuration);
            }
            if (configuration.WithReader != null)
            {
                new ObjectReaderWriter(this).Write(configuration);
            }
        }
    }
}