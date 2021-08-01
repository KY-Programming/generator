using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Generator.Models;
using KY.Generator.Reflection;

namespace KY.Generator.AspDotNet.Helpers
{
    public class AspDotNetOptions : ReflectionOptions
    {
        private List<string> apiVersionValue;
        private List<Type> ignoreGenericsValue;
        private bool? fixCasingWithMappingValue;

        public bool HttpGet { get; private set; }
        public string HttpGetRoute { get; private set; }
        public bool HttpPost { get; private set; }
        public string HttpPostRoute { get; private set; }
        public bool HttpPatch { get; private set; }
        public string HttpPatchRoute { get; private set; }
        public bool HttpPut { get; private set; }
        public string HttpPutRoute { get; private set; }
        public bool HttpDelete { get; private set; }
        public string HttpDeleteRoute { get; private set; }

        public bool IsNonAction { get; private set; }
        public bool IsFromServices { get; private set; }
        public bool IsFromHeader { get; private set; }
        public bool IsFromBody { get; private set; }
        public bool IsFromQuery { get; private set; }
        public List<string> ApiVersion => this.GetMerged(this.apiVersionValue, this.Parent?.CastSafeTo<AspDotNetOptions>()?.ApiVersion);
        public string Route { get; private set; }
        public Type Produces { get; private set; }
        public List<Type> IgnoreGenerics => this.GetMerged(this.ignoreGenericsValue, this.Parent?.CastSafeTo<AspDotNetOptions>()?.IgnoreGenerics, this.Parent?.CastSafeTo<AspDotNetOptions>()?.IgnoreGenerics);
        public bool FixCasingWithMapping => this.fixCasingWithMappingValue ?? this.Parent?.CastSafeTo<AspDotNetOptions>()?.FixCasingWithMapping ?? false;

        protected override void Read(Attribute attribute)
        {
            base.Read(attribute);
            switch (attribute.GetType().Name)
            {
                case "HttpGetAttribute":
                    this.HttpGet = true;
                    this.HttpGetRoute = GetRoute(attribute);
                    break;
                case "HttpPostAttribute":
                    this.HttpPost = true;
                    this.HttpPostRoute = GetRoute(attribute);
                    break;
                case "HttpPatchAttribute":
                    this.HttpPatch = true;
                    this.HttpPatchRoute = GetRoute(attribute);
                    break;
                case "HttpPutAttribute":
                    this.HttpPut = true;
                    this.HttpPutRoute = GetRoute(attribute);
                    break;
                case "HttpDeleteAttribute":
                    this.HttpDelete = true;
                    this.HttpDeleteRoute = GetRoute(attribute);
                    break;
                case "NonActionAttribute":
                    this.IsNonAction = true;
                    break;
                case "FromServicesAttribute":
                    this.IsFromServices = true;
                    break;
                case "FromHeaderAttribute":
                    this.IsFromHeader = true;
                    break;
                case "FromBodyAttribute":
                    this.IsFromBody = true;
                    break;
                case "FromQueryAttribute":
                    this.IsFromQuery = true;
                    break;
                case "ApiVersionAttribute":
                    this.apiVersionValue = attribute.GetType().GetProperty("Versions")?
                                                    .GetValue(attribute)?.CastSafeTo<IEnumerable>().OfType<object>()
                                                    .Select(x => x.ToString()).OrderBy(x => x).ToList();
                    break;
                case "RouteAttribute":
                    this.Route = GetRoute(attribute);
                    break;
                case "ProducesResponseTypeAttribute":
                case "ProducesAttribute":
                    this.Produces = GetProduces(attribute) ?? this.Produces;
                    break;
            }
            switch (attribute)
            {
                case GenerateIgnoreGenericAttribute ignoreGenericAttribute:
                    (this.ignoreGenericsValue ??= new List<Type>()).Add(ignoreGenericAttribute.Type);
                    break;
                case GenerateFixCasingWithMappingAttribute:
                    this.fixCasingWithMappingValue = true;
                    break;
            }
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

        public new static AspDotNetOptions Get(MemberInfo member, IOptions caller = null)
        {
            return Get<AspDotNetOptions>(member, caller);
        }

        public new static AspDotNetOptions Get(ParameterInfo parameter, IOptions caller = null)
        {
            return Get<AspDotNetOptions>(parameter, caller);
        }

        public new static AspDotNetOptions Get(Assembly assembly, IOptions caller = null)
        {
            return Get<AspDotNetOptions>(assembly, caller);
        }
    }
}
