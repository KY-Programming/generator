using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.OData.Configuration;
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
        public void Read(ConfigurationBase configurationBase, List<ITransferObject> transferObjects)
        {
            ODataReadConfiguration configuration = (ODataReadConfiguration)configurationBase;

            if (!string.IsNullOrEmpty(configuration.File))
            {
                this.Parse(FileSystem.ReadAllText(configuration.File), transferObjects);
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
                    this.Parse(responseString, transferObjects);
                }
            }
        }

        private void Parse(string xml, List<ITransferObject> list)
        {
            try
            {
                list.Add(new ODataResultTransferObject(xml));
                using (StringReader stringReader = new StringReader(xml))
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    this.Parse(xmlReader, list);
                }
            }
            catch
            {
                Logger.Trace($"oData response: {xml}");
                throw;
            }
        }

        private void Parse(XmlReader xmlReader, List<ITransferObject> list)
        {
            IEdmModel model = CsdlReader.Parse(xmlReader);
            Dictionary<IEdmType, TypeTransferObject> mapping = this.ReadModels(model, list);
            this.ReadServices(model, mapping, list);
        }

        private Dictionary<IEdmType, TypeTransferObject> ReadModels(IEdmModel edmModel, List<ITransferObject> list)
        {
            Dictionary<IEdmType, TypeTransferObject> mapping = new Dictionary<IEdmType, TypeTransferObject>();
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
                    mapping[entityType.AsActualType()] = model;
                    EntityTransferObject entity = new EntityTransferObject
                                                  {
                                                      Name = schemaType.Name,
                                                      Model = model
                                                  };
                    foreach (IEdmProperty edmProperty in entityType.DeclaredProperties)
                    {
                        model.Properties.Add(new PropertyTransferObject
                                             {
                                                 Name = edmProperty.Name,
                                                 Type = this.ToTransferObject(edmProperty.Type.Definition, mapping)
                                             });
                    }
                    foreach (IEdmStructuralProperty key in entityType.DeclaredKey)
                    {
                        PropertyTransferObject property = model.Properties.FirstOrDefault(x => x.Name == key.Name);
                        entity.Keys.Add(new EntityKeyTransferObject
                                        {
                                            Name = key.Name,
                                            Type = this.ToTransferObject(key.Type.Definition, mapping).Clone(),
                                            Property = property
                                        });
                    }
                    // TODO: Add Navigation Properties
                    list.Add(model);
                    list.Add(entity);
                }
            }
            return mapping;
        }

        private void ReadServices(IEdmModel model, Dictionary<IEdmType, TypeTransferObject> mapping, List<ITransferObject> list)
        {
            foreach (IEdmEntityContainerElement entitySet in model.EntityContainer.Elements)
            {
                HttpServiceTransferObject service = new HttpServiceTransferObject
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
                list.Add(service);
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
            foreach (IEdmStructuralProperty property in entity.DeclaredKey)
            {
                action.Parameters.Add(new HttpServiceActionParameterTransferObject { Name = property.Name, Type = this.ToTransferObject(property.Type.Definition, mapping) });
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
                                   this.ToTransferObject(collectionType.ElementType.Definition, mapping)
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