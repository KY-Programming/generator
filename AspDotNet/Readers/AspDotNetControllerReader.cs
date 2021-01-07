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

            Attribute routeAttribute = type.GetCustomAttributes().FirstOrDefault(x => x.GetType().Name == "RouteAttribute");
            controller.Route = routeAttribute?.GetType().GetProperty("Template")?.GetValue(routeAttribute)?.ToString();

            Attribute apiVersionAttribute = type.GetCustomAttributes().FirstOrDefault(x => x.GetType().Name == "ApiVersionAttribute");
            IEnumerable<object> versions = apiVersionAttribute?.GetType().GetProperty("Versions")?.GetValue(apiVersionAttribute) as IEnumerable<object>;
            controller.Version = versions?.Last().ToString();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                if (method.GetCustomAttribute<GenerateIgnoreAttribute>() != null)
                {
                    continue;
                }

                string fallbackRoute = null;
                Type returnType = method.ReturnType.IgnoreGeneric("System.Threading.Tasks", "Task")
                                        .IgnoreGeneric("Microsoft.AspNetCore.Mvc", "IActionResult")
                                        .IgnoreGeneric("Microsoft.AspNetCore.Mvc", "ActionResult");

                // TODO: Accept ProducesAttribute
                IEnumerable<Attribute> responseTypeAttributes = method.GetCustomAttributes().Where(x => x.GetType().Name == "ProducesResponseTypeAttribute");
                foreach (Attribute responseTypeAttribute in responseTypeAttributes)
                {
                    Type responseTypeAttributeType = responseTypeAttribute.GetType();
                    int statusCode = (int)responseTypeAttributeType.GetProperty("StatusCode").GetMethod.Invoke(responseTypeAttribute, null);
                    if (statusCode == 200)
                    {
                        returnType = (Type)responseTypeAttributeType.GetProperty("Type").GetMethod.Invoke(responseTypeAttribute, null);
                        break;
                    }
                }

                IEnumerable<Type> typesToIgnore = method.GetCustomAttributes<GenerateIgnoreGenericAttribute>()
                                                        .Concat(type.GetCustomAttributes<GenerateIgnoreGenericAttribute>())
                                                        .Select(x => x.Type);
                returnType = returnType.IgnoreGeneric(typesToIgnore);
                this.modelReader.Read(returnType, transferObjects);
                Attribute methodRouteAttribute = method.GetCustomAttributes().FirstOrDefault(x => x.GetType().Name == "RouteAttribute");
                if (methodRouteAttribute != null)
                {
                    fallbackRoute = methodRouteAttribute.GetType().GetProperty("Template")?.GetValue(methodRouteAttribute)?.ToString();
                }
                foreach (Attribute attribute in method.GetCustomAttributes())
                {
                    Type attributeType = attribute.GetType();
                    HttpServiceActionTransferObject action = new HttpServiceActionTransferObject();
                    action.ReturnType = returnType.ToTransferObject();
                    action.Route = attributeType.GetProperty("Template")?.GetValue(attribute)?.ToString() ?? fallbackRoute;
                    int methodNameIndex = 1;
                    while (true)
                    {
                        string actionName = $"{method.Name}{(methodNameIndex > 1 ? methodNameIndex.ToString() : "")}";
                        if (controller.Actions.All(x => !x.Name.Equals(actionName)))
                        {
                            action.Name = actionName;
                            break;
                        }
                        methodNameIndex++;
                    }
                    ParameterInfo[] parameters = method.GetParameters();
                    foreach (ParameterInfo parameter in parameters)
                    {
                        this.modelReader.Read(parameter.ParameterType, transferObjects);
                    }
                    switch (attributeType.Name)
                    {
                        case "HttpGetAttribute":
                            action.Type = HttpServiceActionTypeTransferObject.Get;
                            break;
                        case "HttpPostAttribute":
                            action.Type = HttpServiceActionTypeTransferObject.Post;
                            break;
                        case "HttpPatchAttribute":
                            action.Type = HttpServiceActionTypeTransferObject.Patch;
                            break;
                        case "HttpPutAttribute":
                            action.Type = HttpServiceActionTypeTransferObject.Put;
                            break;
                        case "HttpDeleteAttribute":
                            action.Type = HttpServiceActionTypeTransferObject.Delete;
                            break;
                        case "ConditionalAttribute":
                        case "DebuggerStepThroughAttribute":
                        case "AsyncStateMachineAttribute":
                        case "AuthorizeAttribute":
                        case "RouteAttribute":
                        case "ProducesAttribute":
                        case "ProducesResponseTypeAttribute":
                        case nameof(GenerateIgnoreGenericAttribute):
                            // Ignore these attributes
                            continue;
                        default:
                            Logger.Warning($"Unknown controller action attribute {attributeType.Name}");
                            continue;
                    }
                    action.RequireBodyParameter = action.Type.IsBodyParameterRequired();
                    foreach (ParameterInfo parameter in parameters)
                    {
                        HttpServiceActionParameterTransferObject actionParameter = new HttpServiceActionParameterTransferObject();
                        actionParameter.Name = parameter.Name;
                        actionParameter.Type = parameter.ParameterType.ToTransferObject();
                        actionParameter.FromBody = this.IsFromBodyParameter(parameter, action.Type);
                        actionParameter.FromQuery = this.IsFromQueryParameter(parameter);
                        actionParameter.Inline = action.Route != null && action.Route.Contains($"{{{parameter.Name}}}");
                        actionParameter.InlineIndex = actionParameter.Inline && action.Route != null ? action.Route.IndexOf($"{{{parameter.Name}}}", StringComparison.Ordinal) : 0;
                        actionParameter.IsOptional = parameter.IsOptional;
                        action.Parameters.Add(actionParameter);
                        if (action.Type == HttpServiceActionTypeTransferObject.Get && actionParameter.Type.Name == "List" && actionParameter.FromQuery)
                        {
                            Logger.Error($"HttpGet methods with list parameter {parameter.Name} of {type.FullName}.{method.Name} has to be decorated with [FromQuery]");
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