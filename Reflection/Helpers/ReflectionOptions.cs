using System;
using System.Collections.Generic;
using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Reflection
{
    public class ReflectionOptions : Options
    {
        protected static Dictionary<string, Action<Attribute, Dictionary<string, object>>> Readers { get; } = new();

        static ReflectionOptions()
        {
            Readers.Add(nameof(GenerateIgnoreAttribute), (_, records) => records.Add(nameof(Ignore), true));
            Readers.Add(nameof(GeneratePreferInterfacesAttribute), (_, records) => records.Add(nameof(PreferInterfaces), true));
            Readers.Add(nameof(GenerateStrictAttribute), (_, records) => records.Add(nameof(Strict), true));
        }

        public static ReflectionOptions Get(MemberInfo member)
        {
            return Get<ReflectionOptions>(member);
        }

        protected static T Get<T>(MemberInfo member)
            where T : IOptions, new()
        {
            if (Cache.ContainsKey(member))
            {
                return (T)Cache[member];
            }
            T options = Read<T>(member.GetCustomAttributes(), Get(member.DeclaringType));
            Cache.Add(member, options);
            return options;
        }

        public static ReflectionOptions Get(MethodInfo method)
        {
            return Get<ReflectionOptions>(method);
        }

        protected static T Get<T>(MethodInfo method)
            where T : IOptions, new()
        {
            if (Cache.ContainsKey(method))
            {
                return (T)Cache[method];
            }
            T options = Read<T>(method.GetCustomAttributes(), Get(method.DeclaringType));
            Cache.Add(method, options);
            return options;
        }

        public static ReflectionOptions Get(ParameterInfo parameter)
        {
            return Get<ReflectionOptions>(parameter);
        }

        protected static T Get<T>(ParameterInfo parameter)
            where T : IOptions, new()
        {
            if (Cache.ContainsKey(parameter))
            {
                return (T)Cache[parameter];
            }
            T options = Read<T>(parameter.GetCustomAttributes(), Get(parameter.Member));
            Cache.Add(parameter, options);
            return options;
        }

        public static ReflectionOptions Get(Type type)
        {
            return Get<ReflectionOptions>(type);
        }

        protected static T Get<T>(Type type)
            where T : IOptions, new()
        {
            if (Cache.ContainsKey(type))
            {
                return (T)Cache[type];
            }
            T options = Read<T>(type.GetCustomAttributes(), Get(type.Assembly));
            Cache.Add(type, options);
            return options;
        }

        public static ReflectionOptions Get(Assembly assembly)
        {
            return Get<ReflectionOptions>(assembly);
        }

        protected static T Get<T>(Assembly assembly)
            where T : IOptions, new()
        {
            if (Cache.ContainsKey(assembly))
            {
                return (T)Cache[assembly];
            }
            T options = Read<T>(assembly.GetCustomAttributes(), Global);
            Cache.Add(assembly, options);
            return options;
        }

        protected static T Read<T>(IEnumerable<Attribute> attributes, IOptions parent)
            where T : IOptions, new()
        {
            T options = new();
            if (options is ICoreOptions coreOptions)
            {
                coreOptions.Parent = parent;
                foreach (Attribute attribute in attributes)
                {
                    string key = attribute.GetType().Name;
                    if (Readers.ContainsKey(key))
                    {
                        Readers[key].Invoke(attribute, coreOptions.Records);
                    }
                }
            }
            return options;
        }
    }
}
