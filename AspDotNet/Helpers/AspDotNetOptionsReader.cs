using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Models;

namespace KY.Generator.AspDotNet.Helpers
{
    public static class AspDotNetOptionsReader
    {
        public static AspDotNetOptionsPart Read(Options entry)
        {
            AspDotNetOptionsPart options = new();
            foreach (object attribute in entry.Target.GetCustomAttributes(true))
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
                        options.ApiVersion = attribute.GetType().GetProperty("Versions")?
                                                           .GetValue(attribute)?.CastSafeTo<IEnumerable>().OfType<object>()
                                                           .Select(x => x.ToString()).OrderBy(x => x).ToList();
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
                        (options.IgnoreGenerics ??= new List<Type>()).Add(ignoreGenericAttribute.Type);
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
}
