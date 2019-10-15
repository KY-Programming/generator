using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Reflection.Language;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Extensions;

namespace KY.Generator.AspDotNet.Readers
{
    internal class AspDotNetControllerReader
    {
        private readonly ReflectionModelReader modelReader;

        public AspDotNetControllerReader(ReflectionModelReader modelReader)
        {
            this.modelReader = modelReader;
        }

        public IEnumerable<ITransferObject> Read(AspDotNetReadConfiguration configuration)
        {
            Logger.Trace("Read ASP.net controller...");
            List<Assembly> assemblies = new List<Assembly>();
            if (!string.IsNullOrEmpty(configuration.Controller.Assembly))
            {
                //TODO: Support only assembly name too
                Assembly assembly = Assembly.LoadFile(configuration.Controller.Assembly);
                assemblies.Add(assembly);
            }
            else
            {
                AppDomain.CurrentDomain.GetAssemblies().ForEach(assemblies.Add);
            }

            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(string.Join(".", configuration.Controller.Namespace, configuration.Controller.Name));
                if (type == null)
                {
                    continue;
                }

                HttpServiceTransferObject controller = new HttpServiceTransferObject();
                controller.Name = type.Name;
                controller.Language = ReflectionLanguage.Instance;

                Attribute routeAttribute = type.GetCustomAttributes().FirstOrDefault();
                controller.Route = routeAttribute?.GetType().GetProperty("Template")?.GetValue(routeAttribute)?.ToString();

                MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (MethodInfo method in methods)
                {
                    foreach (ModelTransferObject model in this.modelReader.Read(method.ReturnType))
                    {
                        yield return model;
                    }
                    foreach (Attribute attribute in method.GetCustomAttributes())
                    {
                        Type attributeType = attribute.GetType();
                        HttpServiceActionTransferObject action = new HttpServiceActionTransferObject();
                        action.ReturnType = method.ReturnType.ToTransferObject();
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
                            foreach (ModelTransferObject model in this.modelReader.Read(parameter.ParameterType))
                            {
                                yield return model;
                            }
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

                yield return controller;
            }
        }
    }
}