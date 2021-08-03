using System;
using System.Collections.Generic;
using KY.Generator.Reflection;

namespace KY.Generator.AspDotNet.Helpers
{
    public interface IAspDotNetOptions : IReflectionOptions
    {
        bool HttpGet { get; }
        string HttpGetRoute { get; }
        bool HttpPost { get; }
        string HttpPostRoute { get; }
        bool HttpPatch { get; }
        string HttpPatchRoute { get; }
        bool HttpPut { get; }
        string HttpPutRoute { get; }
        bool HttpDelete { get; }
        string HttpDeleteRoute { get; }
        bool IsNonAction { get; }
        bool IsFromServices { get; }
        bool IsFromHeader { get; }
        bool IsFromBody { get; }
        bool IsFromQuery { get; }
        List<string> ApiVersion { get; }
        string Route { get; }
        Type Produces { get; }
        List<Type> IgnoreGenerics { get; }
        bool FixCasingWithMapping { get; }
    }
}
