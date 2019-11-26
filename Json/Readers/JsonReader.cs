using System.Collections.Generic;
using System.Linq;
using KY.Core.DataAccess;
using KY.Generator.Configurations;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Language;
using KY.Generator.Json.Transfers;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Json.Readers
{
    internal class JsonReader : ITransferReader
    {
        public void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects)
        {
            JsonReadConfiguration configuration = (JsonReadConfiguration)configurationBase;
            JObject source = JsonConvert.DeserializeObject<JObject>(FileSystem.ReadAllText(PathResolver.Resolve(configuration.Source, configuration)));

            this.ReadModel(null, source, transferObjects);
        }

        private ModelTransferObject ReadModel(string name, JObject source, List<ITransferObject> list)
        {
            ModelTransferObject model = name == null ? new JsonModelTransferObject { Name = "Unknown", Language = JsonLanguage.Instance } : new ModelTransferObject { Name = name, Language = JsonLanguage.Instance };
            list.Add(model);

            foreach (JProperty property in source.Properties())
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    ModelTransferObject propertyModel = this.ReadModel(property.Name, (JObject)property.Value, list);
                    model.Properties.Add(new PropertyTransferObject { Name = propertyModel.Name, Type = propertyModel });
                }
                else if (property.Value.Type == JTokenType.Array)
                {
                    TypeTransferObject listType = new TypeTransferObject { Name = JTokenType.Array.ToString() };
                    model.Properties.Add(new PropertyTransferObject { Name = property.Name, Type = listType });

                    List<JToken> children = property.Value.Children().ToList();
                    if (children.Count == 0 || children.Any(x => x.Type != children.First().Type))
                    {
                        listType.Generics.Add(new GenericAliasTransferObject { Type = new TypeTransferObject { Name = JTokenType.Object.ToString() } });
                    }
                    else if (children.First().Type == JTokenType.Object)
                    {
                        ModelTransferObject entryModel = this.ReadModel(property.Name, (JObject)children.First(), list);
                        listType.Generics.Add(new GenericAliasTransferObject { Type = entryModel });
                    }
                    else
                    {
                        listType.Generics.Add(new GenericAliasTransferObject { Type = new TypeTransferObject { Name = children.First().Type.ToString() } });
                    }
                }
                else
                {
                    model.Properties.Add(new PropertyTransferObject { Name = property.Name, Type = new TypeTransferObject { Name = property.Value.Type.ToString() } });
                }
            }
            return model;
        }
    }
}