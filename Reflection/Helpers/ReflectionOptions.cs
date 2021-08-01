using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Reflection
{
    public class ReflectionOptions : Options
    {
        protected static Dictionary<string, Action<Attribute, Dictionary<string, object>>> Readers { get; } = new();

        protected void Read(IEnumerable<Attribute> attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                this.Read(attribute);
            }
        }

        protected virtual void Read(Attribute attribute)
        {
            switch (attribute)
            {
                case GenerateIgnoreAttribute:
                    this.IgnoreValue = true;
                    break;
                case GeneratePreferInterfacesAttribute:
                    this.PreferInterfacesValue = true;
                    break;
                case GenerateStrictAttribute:
                    this.StrictValue = true;
                    break;
                case GenerateRenameAttribute renameAttribute:
                    this.ReplaceNameValue[renameAttribute.Replace] = renameAttribute.With;
                    break;
            }
        }

        protected static T Get<T>(ICustomAttributeProvider member, IOptions parent = null, IOptions caller = null)
            where T : ReflectionOptions, new()
        {
            OptionsCacheEntry cacheEntry = null;
            T options = null;
            if (Cache.ContainsKey(member))
            {
                cacheEntry = Cache[member];
                options = cacheEntry.Get<T>();
                if (options != null)
                {
                    options.Caller = caller;
                    return options;
                }
            }
            options = new();
            options.Target = member;
            options.Parent = parent;
            options.Caller = caller;
            List<Attribute> attributes = new();
            if (!(member is Assembly assembly && assembly.FullName.StartsWith("System") || member is Type type && type.Namespace.StartsWith("System.")))
            {
                attributes.AddRange(member.GetCustomAttributes(true).OfType<Attribute>());
                options.Read(attributes);
            }
            if (cacheEntry == null)
            {
                cacheEntry = new OptionsCacheEntry(attributes);
                Cache.Add(member, cacheEntry);
            }
            cacheEntry.Add(options);
            return options;
        }

        public static ReflectionOptions Get(MemberInfo member, IOptions caller = null)
        {
            return Get<ReflectionOptions>(member, caller);
        }

        protected static T Get<T>(MemberInfo member, IOptions caller = null)
            where T : ReflectionOptions, new()
        {
            return Get<T>(member, member.DeclaringType == null ? Get(member.GetType().Assembly) : Get(member.DeclaringType), caller);
        }

        public static ReflectionOptions Get(ParameterInfo parameter, IOptions caller = null)
        {
            return Get<ReflectionOptions>(parameter, caller);
        }

        protected static T Get<T>(ParameterInfo parameter, IOptions caller = null)
            where T : ReflectionOptions, new()
        {
            return Get<T>(parameter, Get(parameter.Member), caller);
        }

        public static ReflectionOptions Get(Assembly assembly, IOptions caller = null)
        {
            return Get<ReflectionOptions>(assembly, caller);
        }

        protected static T Get<T>(Assembly assembly, IOptions caller = null)
            where T : ReflectionOptions, new()
        {
            return Get<T>(assembly, Global, caller);
        }
    }
}
