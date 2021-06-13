using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Reflection.Language;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.AspDotNet.Readers
{
    public class AspDotNetControllerReader
    {
        private readonly ReflectionModelReader modelReader;

        public AspDotNetControllerReader(ReflectionModelReader modelReader)
        {
            this.modelReader = modelReader;
        }

        public virtual void Read(AspDotNetReadConfiguration configuration, List<ITransferObject> transferObjects)
        {
            configuration.Controller.AssertIsNotNull($"ASP: {nameof(configuration.Controller)}");
            configuration.Controller.Name.AssertIsNotNull($"ASP: {nameof(configuration.Controller)}.{nameof(configuration.Controller.Name)}");
            configuration.Controller.Namespace.AssertIsNotNull($"ASP: {nameof(configuration.Controller)}.{nameof(configuration.Controller.Namespace)}");
            Logger.Trace($"Read ASP.NET controller {configuration.Controller.Namespace}.{configuration.Controller.Name}...");
            Type type = GeneratorTypeLoader.Get(configuration, configuration.Controller.Assembly, configuration.Controller.Namespace, configuration.Controller.Name);
            if (type == null)
            {
                return;
            }

            HttpServiceTransferObject controller = new HttpServiceTransferObject();
            controller.Name = type.Name;
            controller.Language = ReflectionLanguage.Instance;

            List<Attribute> typeAttributes = type.GetCustomAttributes().ToList();
            Attribute routeAttribute = typeAttributes.FirstOrDefault(x => x.GetType().Name == "RouteAttribute");
            controller.Route = routeAttribute?.GetType().GetProperty("Template")?.GetValue(routeAttribute)?.ToString();

            Attribute apiVersionAttribute = typeAttributes.FirstOrDefault(x => x.GetType().Name == "ApiVersionAttribute");
            IEnumerable<object> versions = apiVersionAttribute?.GetType().GetProperty("Versions")?.GetValue(apiVersionAttribute) as IEnumerable<object>;
            controller.Version = versions?.Last().ToString();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                if (method.GetCustomAttribute<GenerateIgnoreAttribute>() != null)
                {
                    continue;
                }

                List<Attribute> methodAttributes = method.GetCustomAttributes().ToList();
                Dictionary<HttpServiceActionTypeTransferObject, string> actionTypes = this.GetActionTypes(methodAttributes);
                if (actionTypes.Count == 0)
                {
                    Logger.Error($"{type.FullName}.{method.Name} has to be decorated with at least one of [HttpGet], [HttpPost], [HttpPut], [HttpPatch], [HttpDelete] or with [GenerateIgnore].");
                    continue;
                }

                string fallbackRoute = null;
                Type returnType = method.ReturnType.IgnoreGeneric("System.Threading.Tasks", "Task")
                                        .IgnoreGeneric("Microsoft.AspNetCore.Mvc", "IActionResult")
                                        .IgnoreGeneric("Microsoft.AspNetCore.Mvc", "ActionResult")
                                        .IgnoreGeneric("System.Web.Mvc", "ActionResult")
                                        .IgnoreGeneric("System.Web.Mvc", "ContentResult")
                                        .IgnoreGeneric("System.Web.Mvc", "EmptyResult")
                                        .IgnoreGeneric("System.Web.Mvc", "FileContentResult")
                                        .IgnoreGeneric("System.Web.Mvc", "FilePathResult")
                                        .IgnoreGeneric("System.Web.Mvc", "FileResult")
                                        .IgnoreGeneric("System.Web.Mvc", "FileStreamResult")
                                        .IgnoreGeneric("System.Web.Mvc", "JsonResult");

                IEnumerable<Attribute> responseTypeAttributes = methodAttributes.Where(x => x.GetType().Name == "ProducesResponseTypeAttribute" || x.GetType().Name == "ProducesAttribute");
                foreach (Attribute responseTypeAttribute in responseTypeAttributes)
                {
                    Type responseTypeAttributeType = responseTypeAttribute.GetType();
                    int statusCode = (int)responseTypeAttributeType.GetProperty("StatusCode").GetMethod.Invoke(responseTypeAttribute, null);
                    if (statusCode == 200)
                    {
                        returnType = (Type)responseTypeAttributeType.GetProperty("Type").GetMethod.Invoke(responseTypeAttribute, null) ?? returnType;
                    }
                }

                IEnumerable<Type> typesToIgnore = method.GetCustomAttributes<GenerateIgnoreGenericAttribute>()
                                                        .Concat(type.GetCustomAttributes<GenerateIgnoreGenericAttribute>())
                                                        .Select(x => x.Type);
                returnType = returnType.IgnoreGeneric(typesToIgnore);
                this.modelReader.Read(returnType, transferObjects);
                Attribute methodRouteAttribute = methodAttributes.FirstOrDefault(x => x.GetType().Name == "RouteAttribute");
                if (methodRouteAttribute != null)
                {
                    fallbackRoute = methodRouteAttribute.GetType().GetProperty("Template")?.GetValue(methodRouteAttribute)?.ToString();
                }
                foreach (KeyValuePair<HttpServiceActionTypeTransferObject, string> actionType in actionTypes)
                {
                    HttpServiceActionTransferObject action = new HttpServiceActionTransferObject();
                    action.Name = actionTypes.Count == 1 ? method.Name : $"{actionType.Key}{method.Name.FirstCharToUpper()}";
                    action.ReturnType = returnType.ToTransferObject();
                    action.Route = actionType.Value ?? fallbackRoute;
                    action.Type = actionType.Key;
                    ParameterInfo[] parameters = method.GetParameters().Where(parameter => parameter.GetCustomAttributes().All(attribute => attribute.GetType().Name != "FromServicesAttribute")).ToArray();
                    foreach (ParameterInfo parameter in parameters)
                    {
                        this.modelReader.Read(parameter.ParameterType, transferObjects);
                    }
                    action.RequireBodyParameter = action.Type.IsBodyParameterRequired();
                    foreach (ParameterInfo parameter in parameters)
                    {
                        if (parameter.ParameterType.FullName == "System.Threading.CancellationToken")
                        {
                            continue;
                        }
                        string fullRoute = $"{controller.Route}/{action.Route}";
                        HttpServiceActionParameterTransferObject actionParameter = new HttpServiceActionParameterTransferObject();
                        actionParameter.Name = parameter.Name;
                        actionParameter.Type = parameter.ParameterType.ToTransferObject();
                        actionParameter.FromBody = this.IsFromBodyParameter(parameter, action.Type);
                        actionParameter.FromQuery = this.IsFromQueryParameter(parameter);
                        actionParameter.Inline = fullRoute.Contains($"{{{parameter.Name}}}");
                        actionParameter.InlineIndex = actionParameter.Inline && action.Route != null ? action.Route.IndexOf($"{{{parameter.Name}}}", StringComparison.Ordinal) : 0;
                        actionParameter.IsOptional = parameter.IsOptional;
                        action.Parameters.Add(actionParameter);
                        if (action.Type == HttpServiceActionTypeTransferObject.Get && actionParameter.Type.Name == "List" && !actionParameter.FromQuery)
                        {
                            Logger.Error($"HttpGet methods with list parameter '{parameter.Name}' of {type.FullName}.{method.Name} has to be decorated with [FromQuery]");
                        }
                    }
                    if (action.RequireBodyParameter)
                    {
                        if (action.Parameters.Count == 0)
                        {
                            throw new InvalidOperationException($"Can not write {method.Name}. {action.Type} requires at least one parameter, but no parameter found. Use [GenerateIgnore] to skip generation for that method");
                        }
                        if (action.Parameters.Count == 1)
                        {
                            action.Parameters.Single().FromBody = true;
                        }
                        else if (action.Parameters.All(x => !x.FromBody))
                        {
                            throw new InvalidOperationException($"Can not write {method.Name}. {action.Type} requires at least one parameter marked with [FromBody] or can have only one parameter");
                        }
                    }
                    if (action.Parameters.Count(x => x.FromBody) > 1)
                    {
                        throw new InvalidOperationException($"Can not write {method.Name}. {action.Type} can have only one parameter marked with [FromBody] or only one reference type");
                    }
                    controller.Actions.Add(action);
                }
            }
            transferObjects.Add(controller);
        }

        private Dictionary<HttpServiceActionTypeTransferObject, string> GetActionTypes(List<Attribute> methodAttributes)
        {
            return methodAttributes.Select(attribute =>
                                   {
                                       Type attributeType = attribute.GetType();
                                       string route = attributeType.GetProperty("Template")?.GetValue(attribute)?.ToString();
                                       switch (attributeType.Name)
                                       {
                                           case "HttpGetAttribute":
                                               return Tuple.Create(HttpServiceActionTypeTransferObject.Get, route);
                                           case "HttpPostAttribute":
                                               return Tuple.Create(HttpServiceActionTypeTransferObject.Post, route);
                                           case "HttpPatchAttribute":
                                               return Tuple.Create(HttpServiceActionTypeTransferObject.Patch, route);
                                           case "HttpPutAttribute":
                                               return Tuple.Create(HttpServiceActionTypeTransferObject.Put, route);
                                           case "HttpDeleteAttribute":
                                               return Tuple.Create(HttpServiceActionTypeTransferObject.Delete, route);
                                           case "ConditionalAttribute":
                                           case "DebuggerStepThroughAttribute":
                                           case "AsyncStateMachineAttribute":
                                           case "AuthorizeAttribute":
                                           case "RouteAttribute":
                                           case "ProducesAttribute":
                                           case "ProducesResponseTypeAttribute":
                                           case nameof(GenerateIgnoreGenericAttribute):
                                               // Ignore these attributes
                                               return null;
                                           default:
                                               Logger.Warning($"Unknown controller action attribute {attributeType.Name}");
                                               return null;
                                       }
                                   })
                                   .Where(x => x != null)
                                   .ToDictionary(x => x.Item1, x => x.Item2);
        }

        private bool IsFromBodyParameter(ParameterInfo parameter, HttpServiceActionTypeTransferObject method)
        {
            bool hasAttribute = parameter.GetCustomAttributes().Any(parameterAttribute => parameterAttribute.GetType().Name == "FromBodyAttribute");
            return hasAttribute || method != HttpServiceActionTypeTransferObject.Get && !parameter.ParameterType.IsValueType && parameter.ParameterType != typeof(string);
        }

        private bool IsFromQueryParameter(ParameterInfo parameter)
        {
            return parameter.GetCustomAttributes().Any(parameterAttribute => parameterAttribute.GetType().Name == "FromQueryAttribute");
        }
    }
}