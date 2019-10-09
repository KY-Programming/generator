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

                AspDotNetController controller = new AspDotNetController();
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
                        AspDotNetControllerAction action = new AspDotNetControllerAction();
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
                        if (attributeType.Name == "HttpGetAttribute")
                        {
                            action.Type = AspDotNetControllerActionType.Get;
                            foreach (ParameterInfo parameter in parameters)
                            {
                                AspDotNetControllerActionParameter actionParameter = new AspDotNetControllerActionParameter();
                                actionParameter.Name = parameter.Name;
                                actionParameter.Type = parameter.ParameterType.ToTransferObject();
                                action.Parameters.Add(actionParameter);
                            }
                        }
                        else if (attributeType.Name == "HttpPatchAttribute")
                        {
                            action.Type = AspDotNetControllerActionType.Patch;
                            foreach (ParameterInfo parameter in parameters)
                            {
                                AspDotNetControllerActionParameter actionParameter = new AspDotNetControllerActionParameter();
                                actionParameter.Name = parameter.Name;
                                actionParameter.Type = parameter.ParameterType.ToTransferObject();
                                action.Parameters.Add(actionParameter);
                            }
                        }
                        else if (attributeType.Name == "HttpPutAttribute")
                        {
                            action.Type = AspDotNetControllerActionType.Put;
                            foreach (ParameterInfo parameter in parameters)
                            {
                                AspDotNetControllerActionParameter actionParameter = new AspDotNetControllerActionParameter();
                                actionParameter.Name = parameter.Name;
                                actionParameter.Type = parameter.ParameterType.ToTransferObject();
                                action.Parameters.Add(actionParameter);
                            }
                        }
                        else if (attributeType.Name == "HttpDeleteAttribute")
                        {
                            action.Type = AspDotNetControllerActionType.Delete;
                            foreach (ParameterInfo parameter in parameters)
                            {
                                AspDotNetControllerActionParameter actionParameter = new AspDotNetControllerActionParameter();
                                actionParameter.Name = parameter.Name;
                                actionParameter.Type = parameter.ParameterType.ToTransferObject();
                                action.Parameters.Add(actionParameter);
                            }
                        }
                        else if (attributeType.Name == "HttpPostAttribute")
                        {
                            action.Type = AspDotNetControllerActionType.Post;
                            if (parameters.Length == 0)
                            {
                                throw new InvalidOperationException($"Method {method.Name} has no parameters. Post requires at least one parameter.");
                            }
                            if (parameters.Length > 1)
                            {
                                Logger.Warning($"Method {method.Name} has more than one parameter. Only one parameter is currently supported.");
                            }
                            ParameterInfo bodyParameter = parameters.FirstOrDefault(parameter => parameter.GetCustomAttributes().Any(x => x.GetType().Name == "FromBodyAttribute"));
                            if (bodyParameter == null)
                            {
                                Logger.Warning($"Method {method.Name} has no [FromBody] attribute. Maybe parameter is not filled correctly.");
                                bodyParameter = parameters.First();
                            }
                            AspDotNetControllerActionParameter actionParameter = new AspDotNetControllerActionParameter();
                            actionParameter.Name = bodyParameter.Name;
                            actionParameter.Type = bodyParameter.ParameterType.ToTransferObject();
                            action.Parameters.Add(actionParameter);
                        }
                        else
                        {
                            Logger.Warning($"Unknown controller action attribute {attributeType.Name}");
                            continue;
                        }
                        controller.Actions.Add(action);
                    }
                }

                yield return controller;
            }
        }
    }
}