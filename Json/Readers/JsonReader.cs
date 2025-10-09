using System.Text.RegularExpressions;
using KY.Core.DataAccess;
using KY.Generator.Json.Configurations;
using KY.Generator.Json.Language;
using KY.Generator.Json.Transfers;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Json.Readers;

internal class JsonReader : ITransferReader
{
    private readonly List<ITransferObject> transferObjects;

    public JsonReader(List<ITransferObject> transferObjects)
    {
        this.transferObjects = transferObjects;
    }

    public void Read(JsonReadConfiguration configuration)
    {
        JObject source = JsonConvert.DeserializeObject<JObject>(FileSystem.ReadAllText(FileSystem.Combine(configuration.BasePath, configuration.Source)));
        string name = Regex.Replace(FileSystem.GetFileName(configuration.Source), @"\.json$", string.Empty, RegexOptions.CultureInvariant);
        this.ReadModel(name, source);
    }

    private ModelTransferObject ReadModel(string name, JObject source)
    {
        ModelTransferObject model = new JsonModelTransferObject { Name = name, Language = JsonLanguage.Instance };
        this.transferObjects.Add(model);

        foreach (JProperty property in source.Properties())
        {
            if (property.Value.Type == JTokenType.Object)
            {
                ModelTransferObject propertyModel = this.ReadModel(property.Name, (JObject)property.Value);
                model.Properties.Add(new PropertyTransferObject
                                     {
                                         Name = propertyModel.Name,
                                         Type = propertyModel,
                                         DeclaringType = model
                                     });
            }
            else if (property.Value.Type == JTokenType.Array)
            {
                TypeTransferObject listType = new() { Name = JTokenType.Array.ToString() };
                model.Properties.Add(new PropertyTransferObject
                                     {
                                         Name = property.Name,
                                         Type = listType,
                                         DeclaringType = model
                                     });

                List<JToken> children = property.Value.Children().ToList();
                if (children.Count == 0 || children.Any(x => x.Type != children.First().Type))
                {
                    listType.Generics.Add(new GenericAliasTransferObject { Type = new TypeTransferObject { Name = JTokenType.Object.ToString() } });
                }
                else if (children.First().Type == JTokenType.Object)
                {
                    ModelTransferObject entryModel = this.ReadModel(property.Name, (JObject)children.First());
                    listType.Generics.Add(new GenericAliasTransferObject { Type = entryModel });
                }
                else
                {
                    listType.Generics.Add(new GenericAliasTransferObject { Type = new TypeTransferObject { Name = children.First().Type.ToString() } });
                }
            }
            else
            {
                model.Properties.Add(new PropertyTransferObject
                                     {
                                         Name = property.Name,
                                         Type = new TypeTransferObject { Name = property.Value.Type.ToString() },
                                         DeclaringType = model
                                     });
            }
        }
        return model;
    }
}
