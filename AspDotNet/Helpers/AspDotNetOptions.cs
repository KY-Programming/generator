using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Reflection;

namespace KY.Generator.AspDotNet.Helpers
{
    public class AspDotNetOptions : ReflectionOptions
    {

        public bool HttpGet => this.GetRecord<bool>(nameof(this.HttpGet));
        public string HttpGetRoute => this.GetRecord<string>(nameof(this.HttpGetRoute));
        public bool HttpPost => this.GetRecord<bool>(nameof(this.HttpPost));
        public string HttpPostRoute => this.GetRecord<string>(nameof(this.HttpPostRoute));
        public bool HttpPatch => this.GetRecord<bool>(nameof(this.HttpPatch));
        public string HttpPatchRoute => this.GetRecord<string>(nameof(this.HttpPatchRoute));
        public bool HttpPut => this.GetRecord<bool>(nameof(this.HttpPut));
        public string HttpPutRoute => this.GetRecord<string>(nameof(this.HttpPutRoute));
        public bool HttpDelete => this.GetRecord<bool>(nameof(this.HttpDelete));
        public string HttpDeleteRoute => this.GetRecord<string>(nameof(this.HttpDeleteRoute));

        public bool IsNonAction => this.GetRecord<bool>(nameof(this.IsNonAction));
        public bool IsFromServices => this.GetRecord<bool>(nameof(this.IsFromServices));
        public bool IsFromHeader => this.GetRecord<bool>(nameof(this.IsFromHeader));
        public bool IsFromBody => this.GetRecord<bool>(nameof(this.IsFromBody));
        public bool IsFromQuery => this.GetRecord<bool>(nameof(this.IsFromQuery));
        public List<string> ApiVersion => this.GetRecord<List<string>>(nameof(this.ApiVersion));
        public string Route => this.GetRecord<string>(nameof(this.Route));
        public Type Produces => this.GetRecord<Type>(nameof(this.Produces));

        static AspDotNetOptions()
        {
            Readers.Add("HttpGetAttribute", (attribute, records) =>
            {
                records.Add(nameof(HttpGet), true);
                records.Add(nameof(HttpGetRoute), GetRoute(attribute));
            });
            Readers.Add("HttpPostAttribute", (attribute, records) =>
            {
                records.Add(nameof(HttpPost), true);
                records.Add(nameof(HttpPostRoute), GetRoute(attribute));
            });
            Readers.Add("HttpPatchAttribute", (attribute, records) =>
            {
                records.Add(nameof(HttpPatch), true);
                records.Add(nameof(HttpPatchRoute), GetRoute(attribute));
            });
            Readers.Add("HttpPutAttribute", (attribute, records) =>
            {
                records.Add(nameof(HttpPut), true);
                records.Add(nameof(HttpPutRoute), GetRoute(attribute));
            });
            Readers.Add("HttpDeleteAttribute", (attribute, records) =>
            {
                records.Add(nameof(HttpDelete), true);
                records.Add(nameof(HttpDeleteRoute), GetRoute(attribute));
            });

            Readers.Add("FromServicesAttribute", (_, records) => records.Add(nameof(IsFromServices), true));
            Readers.Add("FromHeaderAttribute", (_, records) => records.Add(nameof(IsFromHeader), true));
            Readers.Add("FromBodyAttribute", (_, records) => records.Add(nameof(IsFromBody), true));
            Readers.Add("FromQueryAttribute", (_, records) => records.Add(nameof(IsFromQuery), true));
            Readers.Add("ApiVersionAttribute", (attribute, records) => records.Add(
                            nameof(ApiVersion),
                            attribute.GetType().GetProperty("Versions")?.GetValue(attribute)?.CastSafeTo<IEnumerable>().OfType<object>().Select(x => x.ToString()).OrderBy(x => x).ToList()
                        ));
            Readers.Add("RouteAttribute", (attribute, records) => records.Add(nameof(Route), GetRoute(attribute)));
            Readers.Add("ProducesResponseTypeAttribute", (attribute, records) => records.SetNullCoalescing(nameof(Produces), GetProduces(attribute)));
            Readers.Add("ProducesAttribute", (attribute, records) => records.SetNullCoalescing(nameof(Produces), GetProduces(attribute)));
        }

        private static string GetRoute(Attribute attribute)
        {
            return attribute.GetType().GetProperty("Template")?.GetValue(attribute)?.ToString();
        }

        private static Type GetProduces(Attribute attribute)
        {
            Type type = attribute.GetType();
            int? statusCode = (int?)type.GetProperty("StatusCode")?.GetMethod.Invoke(attribute, null);
            if (statusCode == 200)
            {
                return (Type)type.GetProperty("Type")?.GetMethod.Invoke(attribute, null);
            }
            return null;
        }

        public new static AspDotNetOptions Get(MemberInfo member)
        {
            return Get<AspDotNetOptions>(member);
        }

        public new static AspDotNetOptions Get(ParameterInfo parameter)
        {
            return Get<AspDotNetOptions>(parameter);
        }

        public new static AspDotNetOptions Get(MethodInfo method)
        {
            return Get<AspDotNetOptions>(method);
        }

        public new static AspDotNetOptions Get(Type type)
        {
            return Get<AspDotNetOptions>(type);
        }

        public new static AspDotNetOptions Get(Assembly assembly)
        {
            return Get<AspDotNetOptions>(assembly);
        }
    }
}
