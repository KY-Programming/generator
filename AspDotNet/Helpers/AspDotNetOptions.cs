using System;
using System.Collections.Generic;
using System.Reflection;
using KY.Generator.Models;
using KY.Generator.Reflection;

namespace KY.Generator.AspDotNet.Helpers
{
    public sealed class AspDotNetOptions : ReflectionOptions, IAspDotNetOptions
    {
        public bool HttpGet => this.Get<AspDotNetOptionsPart>()?.HttpGet ?? false;
        public string HttpGetRoute => this.Get<AspDotNetOptionsPart>()?.HttpGetRoute;
        public bool HttpPost => this.Get<AspDotNetOptionsPart>()?.HttpPost ?? false;
        public string HttpPostRoute => this.Get<AspDotNetOptionsPart>()?.HttpPostRoute;
        public bool HttpPatch => this.Get<AspDotNetOptionsPart>()?.HttpPatch ?? false;
        public string HttpPatchRoute => this.Get<AspDotNetOptionsPart>()?.HttpPatchRoute;
        public bool HttpPut => this.Get<AspDotNetOptionsPart>()?.HttpPut ?? false;
        public string HttpPutRoute => this.Get<AspDotNetOptionsPart>()?.HttpPutRoute;
        public bool HttpDelete => this.Get<AspDotNetOptionsPart>()?.HttpDelete ?? false;
        public string HttpDeleteRoute => this.Get<AspDotNetOptionsPart>()?.HttpDeleteRoute;

        public bool IsNonAction => this.Get<AspDotNetOptionsPart>()?.IsNonAction ?? false;
        public bool IsFromServices => this.Get<AspDotNetOptionsPart>()?.IsFromServices ?? false;
        public bool IsFromHeader => this.Get<AspDotNetOptionsPart>()?.IsFromHeader ?? false;
        public bool IsFromBody => this.Get<AspDotNetOptionsPart>()?.IsFromBody ?? false;
        public bool IsFromQuery => this.Get<AspDotNetOptionsPart>()?.IsFromQuery ?? false;
        public List<string> ApiVersion => this.GetMerged<AspDotNetOptionsPart, string>(this.Get<AspDotNetOptionsPart>()?.ApiVersion, x => x?.ApiVersion);
        public string Route => this.Get<AspDotNetOptionsPart>()?.Route;
        public Type Produces => this.Get<AspDotNetOptionsPart>()?.Produces;
        public List<Type> IgnoreGenerics => this.GetMerged<AspDotNetOptionsPart, Type>(this.Get<AspDotNetOptionsPart>()?.IgnoreGenerics, x => x?.IgnoreGenerics);
        public bool FixCasingWithMapping => this.GetPrimitive<AspDotNetOptionsPart, bool>(x => x?.FixCasingWithMapping);

        private AspDotNetOptions(ICustomAttributeProvider target, Options parent, IOptions caller = null)
            : base(target, parent, caller)
        {
            Options entry = GetOrCreate(target, parent);
            if (this.CanRead(target))
            {
                this.Add(entry.GetOrRead(AspDotNetOptionsReader.Read));
            }
        }

        public new static IAspDotNetOptions Get(MemberInfo member, IOptions caller = null)
        {
            return new AspDotNetOptions(member, (Options)Get(member.DeclaringType), caller);
        }

        public new static IAspDotNetOptions Get(ParameterInfo parameter, IOptions caller = null)
        {
            return new AspDotNetOptions(parameter, (Options)Get(parameter.Member), caller);
        }

        public new static IAspDotNetOptions Get(Type type, IOptions caller = null)
        {
            return new AspDotNetOptions(type, (Options)Get(type.Assembly), caller);
        }

        public new static IAspDotNetOptions Get(Assembly assembly, IOptions caller = null)
        {
            return new AspDotNetOptions(assembly, Global, caller);
        }
    }
}
