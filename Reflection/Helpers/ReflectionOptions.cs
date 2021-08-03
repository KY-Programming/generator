using System;
using System.Collections.Generic;
using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Reflection
{
    public class ReflectionOptions : Options, IReflectionOptions
    {
        public bool Strict => this.GetPrimitive<ReflectionOptionsPart, bool>(x => x?.Strict);
        public bool PropertiesToFields => this.GetPrimitive<ReflectionOptionsPart, bool>(x => x?.PropertiesToFields);
        public bool FieldsToProperties => this.GetPrimitive<ReflectionOptionsPart, bool>(x => x?.FieldsToProperties);
        public bool PreferInterfaces => this.GetPrimitive<ReflectionOptionsPart, bool>(x => x?.PreferInterfaces);
        public bool OptionalFields => this.GetPrimitive<ReflectionOptionsPart, bool>(x => x?.OptionalFields);
        public bool OptionalProperties => this.GetPrimitive<ReflectionOptionsPart, bool>(x => x?.OptionalProperties);
        public bool Ignore => this.GetPrimitive<ReflectionOptionsPart, bool>(x => x?.Ignore);
        public Dictionary<string, string> ReplaceName => this.GetMerged<IReflectionOptions, string, string>(this.Get<ReflectionOptionsPart>()?.ReplaceName, x => x?.ReplaceName);

        protected ReflectionOptions(ICustomAttributeProvider target, Options parent, IOptions caller = null)
            : base(target, parent, caller)
        {
            Options entry = GetOrCreate(target, parent);
            if (this.CanRead(target))
            {
                this.Add(entry.GetOrRead(ReflectionOptionsReader.Read));
            }
        }

        public static IReflectionOptions Get(MemberInfo member, IOptions caller = null)
        {
            return new ReflectionOptions(member, (Options)Get(member.DeclaringType), caller);
        }

        public static IReflectionOptions Get(ParameterInfo parameter, IOptions caller = null)
        {
            return new ReflectionOptions(parameter, (Options)Get(parameter.Member), caller);
        }

        public static IReflectionOptions Get(Type type, IOptions caller = null)
        {
            return new ReflectionOptions(type, (Options)Get(type.Assembly), caller);
        }

        public static IReflectionOptions Get(Assembly assembly, IOptions caller = null)
        {
            return new ReflectionOptions(assembly, Global, caller);
        }
    }
}
