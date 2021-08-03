using System;
using System.Collections.Generic;

namespace KY.Generator.AspDotNet.Helpers
{
    public class AspDotNetOptionsPart
    {
        public bool? HttpGet { get; set; }
        public string HttpGetRoute { get; set; }
        public bool? HttpPost { get; set; }
        public string HttpPostRoute { get; set; }
        public bool? HttpPatch { get; set; }
        public string HttpPatchRoute { get; set; }
        public bool? HttpPut { get; set; }
        public string HttpPutRoute { get; set; }
        public bool? HttpDelete { get; set; }
        public string HttpDeleteRoute { get; set; }
        public bool? IsNonAction { get; set; }
        public bool? IsFromServices { get; set; }
        public bool? IsFromHeader { get; set; }
        public bool? IsFromBody { get; set; }
        public bool? IsFromQuery { get; set; }
        public List<string> ApiVersion { get; set; }
        public string Route { get; set; }
        public Type Produces { get; set; }
        public List<Type> IgnoreGenerics { get; set; }
        public bool? FixCasingWithMapping { get; set; }
    }
}
