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

            Attribute routeAttribute = type.GetCustomAttributes().FirstOrDefault();
            controller.Route = routeAttribute?.GetType().GetProperty("Template")?.GetValue(routeAttribute)?.ToString();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                Type returnType = method.ReturnType.IgnoreGeneric("System.Threading.Tasks", "Task")
                                        .IgnoreGeneric("Microsoft.AspNetCore.Mvc", "ActionResult");
                this.modelReader.Read(returnType, transferObjects);
                foreach (Attribute attribute in method.GetCustomAttributes())
                {
                    Type attributeType = attribute.GetType();
                    HttpServiceActionTransferObject action = new HttpServiceActionTransferObject();
                    action.ReturnType = returnType.ToTransferObject();
                    action.Route = attributeType.GetProperty("Template")?.GetValue(attribute)?.ToString();
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
                        actionParameter.FromBody = action.RequireBodyParameter && parameter.GetCustomAttributes().Any(parameterAttribute => parameterAttribute.GetType().Name == "FromBodyAttribute");
                        actionParameter.Inline = action.Route != null && action.Route.Contains($"{{{parameter.Name}}}");
                        actionParameter.InlineIndex = actionParameter.Inline && action.Route != null ? action.Route.IndexOf($"{{{parameter.Name}}}", StringComparison.Ordinal) : 0;
                        actionParameter.IsOptional = parameter.IsOptional;
                        action.Parameters.Add(actionParameter);
                    }
                    if (action.RequireBodyParameter)
                    {
                        if (action.Parameters.Count == 0)
                        {
                            throw new InvalidOperationException($"Can not write {method.Name}. {action.Type} requires at least one parameter, but no parameter found.");
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
                    controller.Actions.Add(action);
                }
            }
            transferObjects.Add(controller);
        }
    }
}