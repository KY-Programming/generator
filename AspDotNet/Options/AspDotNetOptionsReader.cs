using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetOptionsReader
    {
        public void Read(object key, AspDotNetOptionsSet entry)
        {
            if (key is not ICustomAttributeProvider attributeProvider)
            {
                return;
            }
            foreach (object attribute in attributeProvider.GetCustomAttributes(true))
            {
                switch (attribute.GetType().Name)
                {
                    case "HttpGetAttribute":
                        entry.Part.HttpGet = true;
                        entry.Part.HttpGetRoute = GetRoute(attribute);
                        break;
                    case "HttpPostAttribute":
                        entry.Part.HttpPost = true;
                        entry.Part.HttpPostRoute = GetRoute(attribute);
                        break;
                    case "HttpPatchAttribute":
                        entry.Part.HttpPatch = true;
                        entry.Part.HttpPatchRoute = GetRoute(attribute);
                        break;
                    case "HttpPutAttribute":
                        entry.Part.HttpPut = true;
                        entry.Part.HttpPutRoute = GetRoute(attribute);
                        break;
                    case "HttpDeleteAttribute":
                        entry.Part.HttpDelete = true;
                        entry.Part.HttpDeleteRoute = GetRoute(attribute);
                        break;
                    case "NonActionAttribute":
                        entry.Part.IsNonAction = true;
                        break;
                    case "FromServicesAttribute":
                        entry.Part.IsFromServices = true;
                        break;
                    case "FromHeaderAttribute":
                        entry.Part.IsFromHeader = true;
                        break;
                    case "FromBodyAttribute":
                        entry.Part.IsFromBody = true;
                        break;
                    case "FromQueryAttribute":
                        entry.Part.IsFromQuery = true;
                        break;
                    case "ApiVersionAttribute":
                        entry.Part.ApiVersion = attribute.GetType().GetProperty("Versions")?
                                                           .GetValue(attribute)?.CastSafeTo<IEnumerable>().OfType<object>()
                                                           .Select(x => x.ToString()).OrderBy(x => x).ToList();
                        break;
                    case "RouteAttribute":
                        entry.Part.Route = GetRoute(attribute);
                        break;
                    case "ProducesResponseTypeAttribute":
                    case "ProducesAttribute":
                        entry.Part.Produces = GetProduces(attribute) ?? entry.Part.Produces;
                        break;
                }
                switch (attribute)
                {
                    case GenerateIgnoreGenericAttribute ignoreGenericAttribute:
                        (entry.Part.IgnoreGenerics ??= new List<Type>()).Add(ignoreGenericAttribute.Type);
                        break;
                    case GenerateFixCasingWithMappingAttribute:
                        entry.Part.FixCasingWithMapping = true;
                        break;
                }
            }
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
