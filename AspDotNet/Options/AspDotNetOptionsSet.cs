using System;
using System.Collections.Generic;

namespace KY.Generator.AspDotNet
{
    public class AspDotNetOptionsSet : OptionsSetBase<AspDotNetOptionsSet, AspDotNetOptionsPart>, IAspDotNetOptions
    {
        public static AspDotNetOptionsSet GlobalInstance { get; } = new(null, null, null, "global");

        bool IAspDotNetOptions.HttpGet => this.GetGlobalPrimitive(x => x?.HttpGet);
        string IAspDotNetOptions.HttpGetRoute => this.GetGlobalValue(x => x?.HttpGetRoute);
        bool IAspDotNetOptions.HttpPost => this.GetGlobalPrimitive(x => x?.HttpPost);
        string IAspDotNetOptions.HttpPostRoute => this.GetGlobalValue(x => x?.HttpPostRoute);
        bool IAspDotNetOptions.HttpPatch => this.GetGlobalPrimitive(x => x?.HttpPatch);
        string IAspDotNetOptions.HttpPatchRoute => this.GetGlobalValue(x => x?.HttpPatchRoute);
        bool IAspDotNetOptions.HttpPut => this.GetGlobalPrimitive(x => x?.HttpPut);
        string IAspDotNetOptions.HttpPutRoute => this.GetGlobalValue(x => x?.HttpPutRoute);
        bool IAspDotNetOptions.HttpDelete => this.GetGlobalPrimitive(x => x?.HttpDelete);
        string IAspDotNetOptions.HttpDeleteRoute => this.GetGlobalValue(x => x?.HttpDeleteRoute);

        bool IAspDotNetOptions.IsNonAction => this.GetGlobalPrimitive(x => x?.IsNonAction);
        bool IAspDotNetOptions.IsFromServices => this.GetGlobalPrimitive(x => x?.IsFromServices);
        bool IAspDotNetOptions.IsFromHeader => this.GetGlobalPrimitive(x => x?.IsFromHeader);
        bool IAspDotNetOptions.IsFromBody => this.GetGlobalPrimitive(x => x?.IsFromBody);
        bool IAspDotNetOptions.IsFromQuery => this.GetGlobalPrimitive(x => x?.IsFromQuery);
        List<string> IAspDotNetOptions.ApiVersion => this.GetMerged(x => x?.ApiVersion);
        string IAspDotNetOptions.Route => this.GetGlobalValue(x => x?.Route);
        Type IAspDotNetOptions.Produces => this.GetGlobalValue(x => x?.Produces);
        List<Type> IAspDotNetOptions.IgnoreGenerics => this.GetMerged( x => x?.IgnoreGenerics);
        bool IAspDotNetOptions.FixCasingWithMapping => this.GetGlobalPrimitive(x => x?.FixCasingWithMapping);

        public AspDotNetOptionsSet(AspDotNetOptionsSet parent, AspDotNetOptionsSet global, AspDotNetOptionsSet caller = null, object target = null)
            : base(parent, global, caller, target)
        { }
    }
}
