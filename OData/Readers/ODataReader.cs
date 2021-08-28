using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Configurations;
using KY.Generator.OData.Configurations;
using KY.Generator.OData.Language;
using KY.Generator.OData.Transfers;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace KY.Generator.OData.Readers
{
    internal class ODataReader : ITransferReader
    {
        private readonly List<ITransferObject> transferObjects;
        private const string BindingParameterName = "bindingParameter";

        public ODataReader(List<ITransferObject> transferObjects)
        {
            this.transferObjects = transferObjects;
        }

        public void Read(ODataReadConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration.File))
            {
                this.Parse(FileSystem.ReadAllText(configuration.File));
            }
            if (!string.IsNullOrEmpty(configuration.Connection))
            {
                Logger.Trace($"Connect to {configuration.Connection}...");
                HttpWebRequest request = WebRequest.CreateHttp(configuration.Connection);
                request.CookieContainer = new CookieContainer();
                transferObjects.OfType<TransferObject<Cookie>>().ForEach(x => request.CookieContainer.Add(x.Value));
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseString = reader.ReadToEnd();
                    this.Parse(responseString);
                }
            }
        }

        private void Parse(string xml)
        {
            try
            {
                this.transferObjects.Add(new ODataResultTransferObject(xml));
                using (StringReader stringReader = new StringReader(xml))
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    this.Parse(xmlReader);
                }
            }
            catch
            {
                Logger.Trace($"oData response: {xml}");
                throw;
            }
        }

        private void Parse(XmlReader xmlReader)
        {
            IEdmModel model = CsdlReader.Parse(xmlReader);
            Dictionary<IEdmType, TypeTransferObject> mapping = this.ReadModels(model);
            this.ReadServices(model, mapping);
        }

        private Dictionary<IEdmType, TypeTransferObject> ReadModels(IEdmModel edmModel)
        {
            Dictionary<IEdmType, TypeTransferObject> modelMapping = new Dictionary<IEdmType, TypeTransferObject>();
            Dictionary<IEdmType, EntityTransferObject> entityMapping = new Dictionary<IEdmType, EntityTransferObject>();
            foreach (IEdmSchemaType schemaType in edmModel.SchemaElements.Select(element => edmModel.FindDeclaredType(element.FullName())).Where(x => x != null))
            {
                if (schemaType.TypeKind == EdmTypeKind.Entity && schemaType is IEdmEntityType entityType)
                {
                    ModelTransferObject model = new ModelTransferObject
                                                {
                                                    Name = schemaType.Name,
                                                    Namespace = schemaType.Namespace,
                                                    Language = ODataLanguage.Instance
                                                };
                    modelMapping[entityType.AsActualType()] = model;
                    EntityTransferObject entity = new EntityTransferObject
                                                  {
                                                      Name = schemaType.Name,
                                                      Model = model
                                                  };
                    entityMapping[entityType.AsActualType()] = entity;
                    foreach (IEdmProperty edmProperty in entityType.DeclaredProperties)
                    {
                        model.Properties.Add(new PropertyTransferObject
                                             {
                                                 Name = edmProperty.Name,
                                                 Type = this.ToTransferObject(edmProperty.Type.Definition, modelMapping)
                                             });
                    }
                    if (entityType.DeclaredKey != null)
                    {
                        foreach (IEdmStructuralProperty key in entityType.DeclaredKey)
                        {
                            PropertyTransferObject property = model.Properties.FirstOrDefault(x => x.Name == key.Name);
                            entity.Keys.Add(new EntityKeyTransferObject
                                            {
                                                Name = key.Name,
                                                Type = this.ToTransferObject(key.Type.Definition, modelMapping).Clone(),
                                                Property = property
                                            });
                        }
                    }
                    // TODO: Add Navigation Properties
                    this.transferObjects.Add(model);
                    this.transferObjects.Add(entity);
                }
            }
            foreach (IEdmAction action in edmModel.SchemaElements.OfType<IEdmAction>())
            {
                IEdmType boundTo = action.FindParameter(BindingParameterName)?.Type.Definition;
                if (boundTo != null && modelMapping.ContainsKey(boundTo))
                {
                    EntityTransferObject entity = entityMapping[boundTo];
                    EntityActionTransferObject entityAction = new EntityActionTransferObject { Name = action.Name, Namespace = action.Namespace };
                    if (action.ReturnType != null)
                    {
                        entityAction.ReturnType = modelMapping[action.ReturnType.Definition];
                    }
                    entity.Actions.Add(entityAction);
                    foreach (IEdmOperationParameter actionParameter in action.Parameters.Where(parameter => parameter.Name != BindingParameterName))
                    {
                        entityAction.Parameters.Add(new EntityActionParameterTransferObject { Name = actionParameter.Name, Type = modelMapping[actionParameter.Type.Definition] });
                    }
                }
                else
                {
                    // TODO: Unbound actions
                }
            }
            return modelMapping;
        }

        private void ReadServices(IEdmModel model, Dictionary<IEdmType, TypeTransferObject> mapping)
        {
            foreach (IEdmEntitySet entitySet in model.EntityContainer.Elements.OfType<IEdmEntitySet>())
            {
                EntitySetTransferObject service = new EntitySetTransferObject
                                                  {
                                                      Name = entitySet.Name,
                                                      Route = entitySet.Name,
                                                      Language = ODataLanguage.Instance
                                                  };
                if (entitySet is IEdmNavigationSource navigationSource)
                {
                    service.Actions.Add(this.ReadAction(HttpServiceActionTypeTransferObject.Get, mapping, navigationSource));
                    service.Actions.Add(this.ReadAction(HttpServiceActionTypeTransferObject.Post, mapping, navigationSource));
                    service.Actions.Add(this.ReadAction(HttpServiceActionTypeTransferObject.Put, mapping, navigationSource));
                    service.Actions.Add(this.ReadAction(HttpServiceActionTypeTransferObject.Patch, mapping, navigationSource));
                    service.Actions.Add(this.ReadAction(HttpServiceActionTypeTransferObject.Delete, mapping, navigationSource));
                }
                service.Entity = this.transferObjects.OfType<EntityTransferObject>().First(x => x.Name == mapping[entitySet.Type.AsElementType()].Name);
                this.transferObjects.Add(service);
            }
        }

        private HttpServiceActionTransferObject ReadAction(HttpServiceActionTypeTransferObject actionType, Dictionary<IEdmType, TypeTransferObject> mapping, IEdmNavigationSource navigationSource)
        {
            HttpServiceActionTransferObject action = new HttpServiceActionTransferObject
                                                     {
                                                         Name = actionType.ToString(),
                                                         Type = actionType,
                                                         Route = "",
                                                         RequireBodyParameter = false
                                                     };
            switch (actionType)
            {
                case HttpServiceActionTypeTransferObject.Get:
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = "query", Type = new TypeTransferObject { Name = "Edm.String" }, AppendName = false });
                    action.ReturnType = this.ToTransferObject(navigationSource.Type, mapping);
                    break;
                case HttpServiceActionTypeTransferObject.Post:
                case HttpServiceActionTypeTransferObject.Put:
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = "model", FromBody = true, Type = this.ToTransferObject(navigationSource.Type, mapping, false) });
                    action.ReturnType = new TypeTransferObject { Name = "Edm.Void" };
                    action.RequireBodyParameter = true;
                    break;
                case HttpServiceActionTypeTransferObject.Patch:
                    this.AddKeysToParameters(mapping, navigationSource, action);
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = "model", FromBody = true, Type = this.ToTransferObject(navigationSource.Type, mapping, false) });
                    action.ReturnType = new TypeTransferObject { Name = "Edm.Void" };
                    action.RequireBodyParameter = true;
                    break;
                case HttpServiceActionTypeTransferObject.Delete:
                    this.AddKeysToParameters(mapping, navigationSource, action);
                    action.ReturnType = new TypeTransferObject { Name = "Edm.Void" };
                    break;
            }
            return action;
        }

        private void AddKeysToParameters(Dictionary<IEdmType, TypeTransferObject> mapping, IEdmNavigationSource navigationSource, HttpServiceActionTransferObject action)
        {
            IEdmEntityType entity = this.ToEntity(navigationSource.Type);
            if (entity.DeclaredKey != null)
            {
                foreach (IEdmStructuralProperty property in entity.DeclaredKey)
                {
                    action.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = property.Name, Type = this.ToTransferObject(property.Type.Definition, mapping) });
                }
            }
        }

        private TypeTransferObject ToTransferObject(IEdmType type, Dictionary<IEdmType, TypeTransferObject> mapping, bool allowCollection = true)
        {
            if (type.TypeKind == EdmTypeKind.Collection && type is IEdmCollectionType collectionType)
            {
                if (allowCollection)
                {
                    return new TypeTransferObject
                           {
                               Name = "Array",
                               Generics =
                               {
                                   new GenericAliasTransferObject { Type = this.ToTransferObject(collectionType.ElementType.Definition, mapping) }
                               }
                           };
                }
                return this.ToTransferObject(collectionType.ElementType.Definition, mapping);
            }
            return mapping.ContainsKey(type) ? mapping[type] :
                   type is IEdmSchemaElement schemaElement ? new TypeTransferObject { Name = schemaElement.Name, Namespace = schemaElement.Namespace } :
                   type is IEdmNamedElement namedElement ? new TypeTransferObject { Name = namedElement.Name } :
                   new TypeTransferObject { Name = type.FullTypeName() };
        }

        private IEdmEntityType ToEntity(IEdmType type)
        {
            return type.TypeKind == EdmTypeKind.Collection && type is IEdmCollectionType collectionType ? (IEdmEntityType)collectionType.ElementType.Definition : (IEdmEntityType)type;
        }
    }
}
