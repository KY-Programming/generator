using System.Collections;
using System.Reflection;
using KY.Core;

namespace KY.Generator.AspDotNet;

public class AspDotNetOptionsFactory : IOptionsFactory
{
    public bool CanCreate(Type optionsType)
    {
        return optionsType == typeof(AspDotNetOptions);
    }

    public object Create(Type optionsType, object key, object? parent, object global)
    {
        return new AspDotNetOptions(parent as AspDotNetOptions, global as AspDotNetOptions, key);
    }

    public object CreateGlobal(Type optionsType, object key, object? parent)
    {
        return key switch
        {
            Assembly assembly => this.CreateFromCustomAttributes(assembly.GetCustomAttributes(), key, parent as AspDotNetOptions),
            MemberInfo member => this.CreateFromCustomAttributes(member.GetCustomAttributes(), key, parent as AspDotNetOptions),
            ParameterInfo parameter => this.CreateFromCustomAttributes(parameter.GetCustomAttributes(), key, parent as AspDotNetOptions),
            Options.RootKey => new AspDotNetOptions(parent as AspDotNetOptions, null, "global"),
            _ => new AspDotNetOptions(parent as AspDotNetOptions, null, key)
            // _ => throw new InvalidOperationException($"Could not create {nameof(AspDotNetOptions)} {key.GetType()}")
        };
    }

    private AspDotNetOptions CreateFromCustomAttributes(IEnumerable<object> customAttributes, object key, AspDotNetOptions? parent)
    {
        AspDotNetOptions options = new(parent, null, key);
        foreach (Attribute attribute in customAttributes)
        {
            switch (attribute.GetType().Name)
            {
                case "HttpGetAttribute":
                    options.HttpGet = true;
                    options.HttpGetRoute = GetRoute(attribute);
                    break;
                case "HttpPostAttribute":
                    options.HttpPost = true;
                    options.HttpPostRoute = GetRoute(attribute);
                    break;
                case "HttpPatchAttribute":
                    options.HttpPatch = true;
                    options.HttpPatchRoute = GetRoute(attribute);
                    break;
                case "HttpPutAttribute":
                    options.HttpPut = true;
                    options.HttpPutRoute = GetRoute(attribute);
                    break;
                case "HttpDeleteAttribute":
                    options.HttpDelete = true;
                    options.HttpDeleteRoute = GetRoute(attribute);
                    break;
                case "NonActionAttribute":
                    options.IsNonAction = true;
                    break;
                case "FromServicesAttribute":
                    options.IsFromServices = true;
                    break;
                case "FromHeaderAttribute":
                    options.IsFromHeader = true;
                    break;
                case "FromBodyAttribute":
                    options.IsFromBody = true;
                    break;
                case "FromQueryAttribute":
                    options.IsFromQuery = true;
                    break;
                case "ApiVersionAttribute":
                    options.AddToApiVersion(
                        attribute.GetType().GetProperty("Versions")?
                                 .GetValue(attribute)?.CastSafeTo<IEnumerable>().OfType<object>()
                                 .Select(x => x.ToString()).OrderBy(x => x)
                    );
                    break;
                case "RouteAttribute":
                    options.Route = GetRoute(attribute);
                    break;
                case "ProducesResponseTypeAttribute":
                case "ProducesAttribute":
                    options.Produces = GetProduces(attribute) ?? options.Produces;
                    break;
            }
            switch (attribute)
            {
                case GenerateIgnoreGenericAttribute ignoreGenericAttribute:
                    options.AddToIgnoreGenerics(ignoreGenericAttribute.Type);
                    break;
                case GenerateFixCasingWithMappingAttribute:
                    options.FixCasingWithMapping = true;
                    break;
            }
        }
        return options;
    }

    private static string GetRoute(object attribute)
    {
        return attribute.GetType().GetProperty("Template")?.GetValue(attribute)?.ToString();
    }

    private static Type GetProduces(object attribute)
    {
        Type type = attribute.GetType();
        int? statusCode = (int?)type.GetProperty("StatusCode")?.GetMethod.Invoke(attribute, null);
        if (statusCode == 200)
        {
            return (Type)type.GetProperty("Type")?.GetMethod.Invoke(attribute, null);
        }
        return null;
    }
}
