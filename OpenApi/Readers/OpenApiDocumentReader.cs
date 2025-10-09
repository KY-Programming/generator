using KY.Generator.OpenApi.Configurations;
using KY.Generator.OpenApi.Extensions;
using KY.Generator.OpenApi.Languages;
using KY.Generator.Transfer;
using Microsoft.OpenApi.Models;

namespace KY.Generator.OpenApi.Readers;

public class OpenApiDocumentReader
{
    private readonly List<ITransferObject> transferObjects;

    public OpenApiDocumentReader(List<ITransferObject> transferObjects)
    {
        this.transferObjects = transferObjects;
    }

    public void Read(OpenApiReadConfiguration configuration)
    {
        foreach (OpenApiDocument document in this.transferObjects.OfType<TransferObject<OpenApiDocument>>().Select(x => x.Value).Where(x => x.Components != null).ToList())
        {
            foreach (OpenApiSchema type in document.Components.Schemas.Values)
            {
                this.Read(configuration, type);
            }
            this.Read(configuration, document);
        }
    }

    private void Read(OpenApiReadConfiguration configuration, OpenApiDocument document)
    {
        HttpServiceTransferObject service = new()
                                            {
                                                Name = configuration.Name ?? document.Info.Description.Replace("Class ", "").Trim()
                                            };
        foreach (KeyValuePair<string, OpenApiPathItem> pathPair in document.Paths)
        {
            foreach (KeyValuePair<OperationType, OpenApiOperation> operationPair in pathPair.Value.Operations)
            {
                HttpServiceActionTypeTransferObject type = operationPair.Key.ToActionType();
                HttpServiceActionTransferObject action = new()
                                                         {
                                                             Name = pathPair.Key.Split('/').Last(),
                                                             Route = pathPair.Key,
                                                             Type = type,
                                                             RequireBodyParameter = type.IsBodyParameterRequired()
                                                         };
                foreach (OpenApiParameter parameter in operationPair.Value.Parameters)
                {
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject
                                          {
                                              Name = parameter.Name,
                                              Type = this.Read(configuration, parameter.Schema)
                                          });
                }
                OpenApiMediaType content = operationPair.Value.RequestBody.Content.Single().Value;
                if (action.RequireBodyParameter)
                {
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject
                                          {
                                              Name = "request",
                                              Type = this.Read(configuration, content.Schema),
                                              FromBody = true
                                          });
                }
                else
                {
                    foreach (KeyValuePair<string, OpenApiSchema> propertyPair in content.Schema.Properties)
                    {
                        action.Parameters.Add(new HttpServiceActionParameterTransferObject
                                              {
                                                  Name = propertyPair.Key,
                                                  Type = this.Read(configuration, propertyPair.Value)
                                              });
                    }
                }
                OpenApiResponse response = operationPair.Value.Responses.SingleOrDefault(x => x.Key == "200").Value;
                if (response == null)
                {
                    action.ReturnType = new TypeTransferObject { Name = "void" };
                }
                else
                {
                    action.ReturnType = this.Read(configuration, response.Content.Single().Value.Schema);
                }
                service.Actions.Add(action);
            }
        }
    }

    private TypeTransferObject Read(OpenApiReadConfiguration configuration, OpenApiSchema schema)
    {
        if (schema.Reference == null)
        {
            return new TypeTransferObject
                   {
                       Name = schema.Type
                   };
        }

        ModelTransferObject model = this.transferObjects.OfType<ModelTransferObject>().FirstOrDefault(x => x.Name == schema.Reference.Id);
        if (model != null)
        {
            return model;
        }

        if (schema.Type == "array")
        {
            return new TypeTransferObject
                   {
                       Name = schema.Type,
                       Generics =
                       {
                           new GenericAliasTransferObject { Type = this.Read(configuration, schema.Items) }
                       }
                   };
        }
        if (schema.Type == "object")
        {
            model = new ModelTransferObject
                    {
                        Name = schema.Reference.Id,
                        Namespace = configuration.Namespace,
                        Language = OpenApiLanguage.Instance
                    };
            foreach (KeyValuePair<string, OpenApiSchema> propertyPair in schema.Properties)
            {
                PropertyTransferObject property = new()
                                                  {
                                                      Name = propertyPair.Key,
                                                      Type = this.Read(configuration, propertyPair.Value),
                                                      DeclaringType = model
                                                  };

                if (configuration.DataAnnotations && propertyPair.Value.MaxLength.HasValue)
                {
                    property.Attributes.Add(new AttributeTransferObject
                                            {
                                                Name = "MaxLength",
                                                Namespace = "System.ComponentModel.DataAnnotations",
                                                Value = propertyPair.Value.MaxLength.Value
                                            });
                }
                model.Properties.Add(property);
            }

            this.transferObjects.Add(model);
            return model;
        }
        throw new InvalidOperationException($"Unknown schema type {schema.Type}");
    }
}
