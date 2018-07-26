using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using KY.Core.Xml;

namespace KY.Generator.OData.MetaData
{
    public class ODataMetaDataReader
    {
        private static readonly Regex collectionRegex = new Regex(@"^Collection\((?<type>[^)]+)\)");
        private readonly string connection;
        private readonly List<Cookie> cookies;

        public ODataMetaDataReader(string connection, List<Cookie> cookies = null)
        {
            this.connection = connection;
            this.cookies = cookies;
        }

        public ODataMetaData Read()
        {
            HttpWebRequest request = WebRequest.CreateHttp(this.connection);
            if (this.cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                this.cookies.ForEach(request.CookieContainer.Add);
            }
            WebResponse response = request.GetResponse();
            XElement root;
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string responseString = reader.ReadToEnd();
                root = XElement.Parse(responseString);
            }
            ODataMetaData metaData = new ODataMetaData();
            XElement dataServicesElement = root.GetElementIgnoreNamespace("DataServices");
            if (dataServicesElement != null)
            {
                Dictionary<string, ODataMetaDataEntityType> entityTypeMapping = new Dictionary<string, ODataMetaDataEntityType>();
                IList<XElement> schemaElements = dataServicesElement.GetElementsIgnoreNamespace("Schema").ToList();
                foreach (XElement schemaElement in schemaElements)
                {
                    string nameSpace = schemaElement.TryGetStringAttribute("Namespace");
                    List<ODataMetaDataAssociation> associations = this.ReadAssociations(schemaElement).ToList();
                    foreach (XElement entityTypeElement in schemaElement.GetElementsIgnoreNamespace("EntityType"))
                    {
                        ODataMetaDataEntityType entityType = this.ReadEntityType(entityTypeElement, nameSpace, associations);
                        metaData.EntityTypes.Add(entityType);
                        entityTypeMapping.Add(entityType.FullName, entityType);
                    }
                }
                foreach (XElement schemaElement in schemaElements)
                {
                    string nameSpace = schemaElement.TryGetStringAttribute("Namespace");
                    List<ODataMetaDataAction> actions = new List<ODataMetaDataAction>();
                    IEnumerable<XElement> actionElements = schemaElement.GetElementsIgnoreNamespace("Action");
                    foreach (XElement actionElement in actionElements)
                    {
                        actions.Add(this.ReadAction(actionElement));
                    }
                    IEnumerable<XElement> functionElements = schemaElement.GetElementsIgnoreNamespace("Function");
                    foreach (XElement functionElement in functionElements)
                    {
                        actions.Add(this.ReadFunction(functionElement));
                    }
                    IEnumerable<XElement> entityContainerElements = schemaElement.GetElementsIgnoreNamespace("EntityContainer");
                    foreach (XElement entityContainerElement in entityContainerElements)
                    {
                        metaData.DataContext = this.ReadDataContex(entityContainerElement, nameSpace, entityTypeMapping, actions);
                    }
                }
            }
            return metaData;
        }

        private ODataMetaDataAction ReadAction(XElement actionElement)
        {
            ODataMetaDataAction action = new ODataMetaDataAction();
            this.ReadActionProperties(actionElement, action);
            return action;
        }

        private void ReadActionProperties(XElement actionElement, ODataMetaDataAction action)
        {
            action.Name = actionElement.TryGetStringAttribute(nameof(ODataMetaDataAction.Name));
            action.IsBound = bool.TrueString.Equals(actionElement.TryGetStringAttribute(nameof(ODataMetaDataAction.IsBound)), StringComparison.CurrentCultureIgnoreCase);
            IEnumerable<XElement> parameterElements = actionElement.GetElementsIgnoreNamespace("Parameter");
            foreach (XElement parameterElement in parameterElements)
            {
                ODataMetaDataActionParameter parameter = new ODataMetaDataActionParameter();
                parameter.Name = parameterElement.TryGetStringAttribute(nameof(ODataMetaDataActionParameter.Name));
                parameter.Type = parameterElement.TryGetStringAttribute(nameof(ODataMetaDataActionParameter.Type));
                if (parameter.Name.Equals("bindingParameter", StringComparison.CurrentCultureIgnoreCase))
                {
                    action.BoundTo = parameter.Type;
                }
                else
                {
                    action.Parameters.Add(parameter);
                }
            }
        }

        private ODataMetaDataFunction ReadFunction(XElement functionElement)
        {
            ODataMetaDataFunction function = new ODataMetaDataFunction();
            this.ReadActionProperties(functionElement, function);

            XElement returnTypeElement = functionElement.GetElementIgnoreNamespace("ReturnType");
            if (returnTypeElement != null)
            {
                function.ReturnType = new ODataMataDataFunctionReturnType();
                function.ReturnType.Type = returnTypeElement.TryGetStringAttribute(nameof(ODataMataDataFunctionReturnType.Type));
                function.ReturnType.Nullable = bool.TrueString.Equals(returnTypeElement.TryGetStringAttribute(nameof(ODataMataDataFunctionReturnType.Nullable)), StringComparison.CurrentCultureIgnoreCase);
            }
            return function;
        }

        private ODataMetaDataEntityType ReadEntityType(XElement entityTypeElement, string nameSpace, List<ODataMetaDataAssociation> associations)
        {
            ODataMetaDataEntityType entityType = new ODataMetaDataEntityType();
            entityType.Name = entityTypeElement.TryGetStringAttribute(nameof(ODataMetaDataEntityType.Name));
            entityType.Namespace = nameSpace == "Default" ? null : nameSpace;
            IEnumerable<XElement> propertyElements = entityTypeElement.GetElementsIgnoreNamespace("Property");
            foreach (XElement propertyElement in propertyElements)
            {
                ODataMetaDataProperty property = new ODataMetaDataProperty();
                property.Name = propertyElement.TryGetStringAttribute("Name");
                property.Type = propertyElement.TryGetStringAttribute("Type");
                property.Nullable = propertyElement.TryGetBoolAttribute("Nullable");
                entityType.Properties.Add(property);
            }
            XElement keyElement = entityTypeElement.GetElementIgnoreNamespace("Key");
            if (keyElement != null)
            {
                IEnumerable<XElement> refElements = keyElement.GetElementsIgnoreNamespace("PropertyRef");
                foreach (XElement refElement in refElements)
                {
                    string propertyName = refElement.TryGetStringAttribute("Name");
                    entityType.Keys.Add(entityType.Properties.First(x => x.Name == propertyName));
                }
            }
            IEnumerable<XElement> navigationPropertyElements = entityTypeElement.GetElementsIgnoreNamespace("NavigationProperty");
            foreach (XElement navigationPropertyElement in navigationPropertyElements)
            {
                string type = navigationPropertyElement.TryGetStringAttribute(nameof(ODataMetaDataNavigationProperty.Type));
                ODataMetaDataNavigationProperty navigationProperty = new ODataMetaDataNavigationProperty();
                navigationProperty.Name = navigationPropertyElement.TryGetStringAttribute(nameof(ODataMetaDataNavigationProperty.Name));
                // oData v2
                if (string.IsNullOrEmpty(type))
                {
                    // TODO: Do not ignore namespace in second step
                    string relationship = navigationPropertyElement.TryGetStringAttribute("Relationship")?.Replace(nameSpace + ".", string.Empty);
                    string toRole = navigationPropertyElement.TryGetStringAttribute("ToRole");
                    ODataMetaDataAssociation association = associations.First(x => x.Name == relationship);
                    ODataMetaDataAssociationEnd end = association.Ends.First(x => x.Role == toRole);
                    navigationProperty.Type = end.Type;
                    navigationProperty.IsCollection = end.Multiplicity != "1";
                }
                // oData v4
                else
                {
                    Match match = collectionRegex.Match(type);
                    type = match.Success ? match.Groups["type"].Value : type;
                    navigationProperty.Type = type;
                    navigationProperty.IsCollection = match.Success;
                }
                entityType.NavigationProperties.Add(navigationProperty);
            }
            return entityType;
        }

        private IEnumerable<ODataMetaDataAssociation> ReadAssociations(XElement schemaElement)
        {
            IEnumerable<XElement> associationElements = schemaElement.GetElementsIgnoreNamespace("Association");
            foreach (XElement associationElement in associationElements)
            {
                ODataMetaDataAssociation association = new ODataMetaDataAssociation();
                association.Name = associationElement.GetStringAttribute(nameof(ODataMetaDataAssociation.Name));
                foreach (XElement endElement in associationElement.GetElementsIgnoreNamespace("End"))
                {
                    ODataMetaDataAssociationEnd end = new ODataMetaDataAssociationEnd();
                    end.Type = endElement.GetStringAttribute(nameof(ODataMetaDataAssociationEnd.Type));
                    end.Role = endElement.GetStringAttribute(nameof(ODataMetaDataAssociationEnd.Role));
                    end.Multiplicity = endElement.GetStringAttribute(nameof(ODataMetaDataAssociationEnd.Multiplicity));
                    association.Ends.Add(end);
                }
                yield return association;
            }
        }

        private ODataMetaDataDataContext ReadDataContex(XElement dataContextElement, string nameSpace, Dictionary<string, ODataMetaDataEntityType> entityTypeMapping, List<ODataMetaDataAction> actions)
        {
            ODataMetaDataDataContext dataContext = new ODataMetaDataDataContext();
            dataContext.Namespace = nameSpace == "Default" ? null : nameSpace;
            dataContext.UnboundActions.AddRange(actions.Where(x => !x.IsBound));
            foreach (XElement entitySetElement in dataContextElement.GetElementsIgnoreNamespace("EntitySet"))
            {
                ODataMetaDataEntitySet entitySet = new ODataMetaDataEntitySet();
                entitySet.Name = entitySetElement.TryGetStringAttribute(nameof(ODataMetaDataEntitySet.Name));
                string entityTypeName = entitySetElement.TryGetStringAttribute(nameof(ODataMetaDataEntitySet.EntityType));
                entitySet.EntityType = entityTypeMapping[entityTypeName];
                entitySet.Actions.AddRange(actions.Where(x => x.IsBound && x.BoundTo.Equals(entitySet.EntityType.FullName)));
                dataContext.EntitySets.Add(entitySet);
            }
            return dataContext;
        }
    }
}