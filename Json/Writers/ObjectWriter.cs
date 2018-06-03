using System.IO;
using KY.Core.DataAccess;
using KY.Generator.Json.Configuration;
using KY.Generator.Json.Extensions;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Json.Writers
{
    internal class ObjectWriter
    {
        private readonly IGenerator generator;
        private readonly ITypeMapping typeMapping;

        public ObjectWriter(IGenerator generator, ITypeMapping typeMapping)
        {
            this.generator = generator;
            this.typeMapping = typeMapping;
        }

        public void Write(JsonConfiguration configuration)
        {
            string name = Path.GetFileNameWithoutExtension(FileSystem.GetFileName(configuration.ToObject.Source));
            JObject source = JsonConvert.DeserializeObject<JObject>(FileSystem.ReadAllText(configuration.ToObject.Source));
            this.Write(source, name, configuration);
        }

        private void Write(JObject source, string name, JsonConfiguration configuration)
        {
            ClassTemplate classTemplate = this.generator.Files.AddFile(configuration.ToObject.RelativePath).AddNamespace(configuration.ToObject.Namespace).AddClass(name);
            configuration.WithReader?.WrittenObjects.Add(classTemplate);
            foreach (JProperty property in source.Properties())
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    classTemplate.AddProperty(property.Name, Code.Type(property.Name));
                    this.Write((JObject)property.Value, property.Name, configuration);
                }
                else
                {
                    classTemplate.AddProperty(property.Name, this.GetType(property, configuration));
                }
            }
        }

        private TypeTemplate GetType(JProperty property, JsonConfiguration configuration)
        {
            return Code.Type(this.typeMapping.Get(property.Value.Type, configuration.Language).ToType);
        }
    }
}