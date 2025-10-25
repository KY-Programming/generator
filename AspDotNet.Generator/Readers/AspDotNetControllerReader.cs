using System.Reflection;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Generator.AspDotNet.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Reflection.Language;
using KY.Generator.Reflection.Readers;
using KY.Generator.Transfer;

namespace KY.Generator.AspDotNet.Readers;

public class AspDotNetControllerReader
{
    private readonly ReflectionModelReader modelReader;
    private readonly Options options;
    private readonly List<ITransferObject> transferObjects;
    private readonly GeneratorTypeLoader typeLoader;

    public AspDotNetControllerReader(ReflectionModelReader modelReader, Options options, List<ITransferObject> transferObjects, GeneratorTypeLoader typeLoader)
    {
        this.modelReader = modelReader;
        this.options = options;
        this.transferObjects = transferObjects;
        this.typeLoader = typeLoader;
    }

    public virtual void Read(AspDotNetReadConfiguration configuration)
    {
        configuration.Controller.AssertIsNotNull($"ASP: {nameof(configuration.Controller)}");
        configuration.Controller.Name.AssertIsNotNull($"ASP: {nameof(configuration.Controller)}.{nameof(configuration.Controller.Name)}");
        configuration.Controller.Namespace.AssertIsNotNull($"ASP: {nameof(configuration.Controller)}.{nameof(configuration.Controller.Namespace)}");
        Logger.Trace($"Read ASP.NET controller {configuration.Controller.Namespace}.{configuration.Controller.Name}...");
        Type? type = this.typeLoader.Get(configuration.Controller.Namespace, configuration.Controller.Name);
        if (type == null)
        {
            return;
        }

        HttpServiceTransferObject controller = new();
        controller.Name = type.Name;
        controller.Language = ReflectionLanguage.Instance;

        AspDotNetOptions typeAspOptions = this.options.Get<AspDotNetOptions>(type);
        controller.Route = typeAspOptions.Route;
        controller.Version = typeAspOptions.ApiVersion?.LastOrDefault();
        this.options.Map(controller, () => this.options.Get<GeneratorOptions>(type, null));

        List<MethodInfo> methods = [];
        Type currentTyp = type;
        while (currentTyp?.Namespace != null && !currentTyp.Namespace.StartsWith("Microsoft") && !currentTyp.Namespace.StartsWith("System"))
        {
            GeneratorOptions currentOptions = this.options.Get<GeneratorOptions>(currentTyp);
            if (currentOptions.Ignore)
            {
                break;
            }
            methods.AddRange(currentTyp.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                       // Remove all overwritten methods
                                       .Where(method => methods.All(m => m.GetBaseDefinition() != method))
            );
            currentTyp = currentTyp.BaseType;
        }
        foreach (MethodInfo method in methods)
        {
            GeneratorOptions methodOptions = this.options.Get<GeneratorOptions>(method);
            AspDotNetOptions methodAspOptions = this.options.Get<AspDotNetOptions>(method);
            if (methodOptions.Ignore || methodAspOptions.IsNonAction)
            {
                continue;
            }

            this.options.Map(method, () => this.options.Get<GeneratorOptions>(method, null));
            Dictionary<HttpServiceActionTypeTransferObject, string> actionTypes = this.GetActionTypes(methodAspOptions);
            if (actionTypes.Count == 0)
            {
                Logger.Error($"{type.FullName}.{method.Name} has to be decorated with at least one of [HttpGet], [HttpPost], [HttpPut], [HttpPatch], [HttpDelete], [NonAction] or with [GenerateIgnore].");
                continue;
            }
            Type returnType = (methodAspOptions.Produces ?? method.ReturnType)
                              .IgnoreGeneric("System.Threading.Tasks", "Task")
                              .IgnoreGeneric("Microsoft.AspNetCore.Mvc", "IActionResult")
                              .IgnoreGeneric("Microsoft.AspNetCore.Mvc", "ActionResult")
                              .IgnoreGeneric("System.Web.Mvc", "ActionResult")
                              .IgnoreGeneric("System.Web.Mvc", "ContentResult")
                              .IgnoreGeneric("System.Web.Mvc", "EmptyResult")
                              .IgnoreGeneric("System.Web.Mvc", "FileContentResult")
                              .IgnoreGeneric("System.Web.Mvc", "FilePathResult")
                              .IgnoreGeneric("System.Web.Mvc", "FileResult")
                              .IgnoreGeneric("System.Web.Mvc", "FileStreamResult")
                              .IgnoreGeneric("System.Web.Mvc", "JsonResult")
                              .IgnoreGeneric(methodAspOptions.IgnoreGenerics);

            Type returnEntryType = returnType.IgnoreGeneric(typeof(IEnumerable<>)).IgnoreGeneric(typeof(List<>)).IgnoreGeneric(typeof(IList<>));
            AspDotNetOptions returnEntryTypeOptions = this.options.Get(returnEntryType, methodAspOptions);
            foreach (KeyValuePair<HttpServiceActionTypeTransferObject, string> actionType in actionTypes)
            {
                HttpServiceActionTransferObject action = new();
                action.Name = actionTypes.Count == 1 ? method.Name : $"{actionType.Key}{method.Name.FirstCharToUpper()}";
                action.ReturnType = this.modelReader.Read(methodOptions.ReturnType, methodOptions) ?? this.modelReader.Read(returnType, methodOptions);
                action.Route = actionType.Value ?? methodAspOptions.Route;
                if (action.Route?.Contains(":") ?? false)
                {
                    action.Route = Regex.Replace(action.Route, "({[^:]*)((:apiVersion)|:[^}?]+)(\\??})", "$1$3$4");
                }
                action.Type = actionType.Key;
                action.Version = methodAspOptions.ApiVersion?.OrderByDescending(x => x).FirstOrDefault();
                action.FixCasingWithMapping = methodOptions.ReturnType == null && returnEntryTypeOptions.FixCasingWithMapping || methodAspOptions.FixCasingWithMapping;
                action.RequireBodyParameter = action.Type.IsBodyParameterRequired();
                action.CanHaveBodyParameter = action.Type.IsBodyParameterAllowed();
                List<ParameterInfo> parameters = method.GetParameters().Where(parameter => !this.options.Get(parameter, methodAspOptions).IsFromHeader
                                                                                           && !this.options.Get(parameter, methodAspOptions).IsFromServices
                                                                                           && parameter.ParameterType.FullName != "System.Threading.CancellationToken"
                ).ToList();
                foreach (ParameterInfo parameter in parameters)
                {
                    AspDotNetOptions parameterOptions = this.options.Get(parameter, methodAspOptions);
                    string fullRoute = $"{controller.Route}/{action.Route}";
                    HttpServiceActionParameterTransferObject actionParameter = new();
                    actionParameter.Name = parameter.Name;
                    actionParameter.Type = this.modelReader.Read(parameter.ParameterType, methodOptions);
                    actionParameter.FromBody = parameterOptions.IsFromBody || action.Type != HttpServiceActionTypeTransferObject.Get && !parameter.ParameterType.IsValueType && parameter.ParameterType != typeof(string);
                    actionParameter.FromQuery = parameterOptions.IsFromQuery;
                    actionParameter.Inline = fullRoute.Contains($"{{{parameter.Name}}}");
                    actionParameter.InlineIndex = actionParameter.Inline && action.Route != null ? action.Route.IndexOf($"{{{parameter.Name}}}") : 0;
                    actionParameter.IsOptional = parameter.IsOptional || Nullable.GetUnderlyingType(parameter.ParameterType) != null;
                    if (fullRoute.Contains($"{{{parameter.Name}?}}"))
                    {
                        actionParameter.Inline = true;
                        actionParameter.IsOptional = true;
                        actionParameter.InlineIndex = fullRoute.IndexOf($"{{{parameter.Name}?}}");
                        action.Route = action.Route.Replace($"{{{parameter.Name}?}}", $"{{{parameter.Name}}}");
                    }
                    action.Parameters.Add(actionParameter);
                    if (action.Type == HttpServiceActionTypeTransferObject.Get && actionParameter.Type.Name == "List" && !actionParameter.FromQuery)
                    {
                        Logger.Error($"HttpGet methods with list parameter '{parameter.Name}' of {type.FullName}.{method.Name} has to be decorated with [FromQuery]");
                    }
                    if (actionParameter.FromBody && !action.CanHaveBodyParameter)
                    {
                        Logger.Warning($"{controller.Name}.{method.Name} with [Http{action.Type}] attribute does not supports [FromBody] parameters");
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
                int tries = 0;
                while (controller.Actions.Any(a => a.Name == action.Name))
                {
                    if (tries > 10)
                    {
                        throw new InvalidOperationException($"Can not find a suitable name for {action.Name}");
                    }
                    if (parameters.Count > 0 && tries == 0)
                    {
                        action.Name += "By" + parameters.First().Name.FirstCharToUpper();
                    }
                    else if (parameters.Count > tries)
                    {
                        action.Name += "And" + parameters.Skip(tries).First().Name.FirstCharToUpper();
                    }
                    else
                    {
                        action.Name += tries - parameters.Count + 1;
                    }
                    tries++;
                }
                controller.Actions.Add(action);
            }
        }
        this.transferObjects.Add(controller);
    }

    private Dictionary<HttpServiceActionTypeTransferObject, string> GetActionTypes(AspDotNetOptions options)
    {
        Dictionary<HttpServiceActionTypeTransferObject, string> dictionary = new();
        if (options.HttpGet)
        {
            dictionary.Add(HttpServiceActionTypeTransferObject.Get, options.HttpGetRoute);
        }
        if (options.HttpPost)
        {
            dictionary.Add(HttpServiceActionTypeTransferObject.Post, options.HttpPostRoute);
        }
        if (options.HttpPatch)
        {
            dictionary.Add(HttpServiceActionTypeTransferObject.Patch, options.HttpPatchRoute);
        }
        if (options.HttpPut)
        {
            dictionary.Add(HttpServiceActionTypeTransferObject.Put, options.HttpPutRoute);
        }
        if (options.HttpDelete)
        {
            dictionary.Add(HttpServiceActionTypeTransferObject.Delete, options.HttpDeleteRoute);
        }
        return dictionary;
    }
}
