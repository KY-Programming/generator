using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;

namespace KY.Generator.Models
{
    public class Options
    {
        private static Dictionary<ICustomAttributeProvider, Options> Cache { get; } = new();
        private Dictionary<Type, object> Parts { get; } = new();

        public static Options Global { get; } = new(null, null);
        public ICustomAttributeProvider Target { get; }
        public Options Parent { get; }
        public Options Caller { get; }

        public Options(ICustomAttributeProvider target, Options parent, IOptions caller = null)
        {
            this.Target = target;
            this.Parent = parent;
            if (caller != null && caller is not Options)
            {
                throw new InvalidOperationException("Caller has to be of type Options");
            }
            this.Caller = (Options)caller;
        }

        protected static Options GetOrCreate(ICustomAttributeProvider target, Options parent)
        {
            Options entry;
            if (Cache.ContainsKey(target))
            {
                entry = Cache[target];
            }
            else
            {
                entry = new Options(target, parent);
                Cache[target] = entry;
            }
            return entry;
        }

        protected bool CanRead(ICustomAttributeProvider target)
        {
            return !(target is Assembly assembly && assembly.FullName.StartsWith("System") || target is Type type && type.Namespace.StartsWith("System"));
        }

        protected TValue GetValue<TPart, TValue>(Func<TPart, TValue> getAction, TValue defaultValue = default)
            where TValue : class
            where TPart : class
        {
            return getAction(this.Get<TPart>())
                   ?? getAction(this.Caller?.Get<TPart>())
                   ?? getAction(this.Parent?.Get<TPart>())
                   ?? defaultValue;
        }

        protected TValue GetPrimitive<TPart, TValue>(Func<TPart, TValue?> getAction, TValue defaultValue = default)
            where TValue : struct
            where TPart : class
        {
            return getAction(this.Get<TPart>())
                   ?? getAction(this.Caller?.Get<TPart>())
                   ?? getAction(this.Parent?.Get<TPart>())
                   ?? defaultValue;
        }

        protected List<TValue> GetMerged<TOptions, TValue>(List<TValue> current, Func<TOptions, List<TValue>> getAction)
            where TOptions : class
        {
            return this.GetMerged(current, getAction(this.Caller?.CastSafeTo<TOptions>()), getAction(this.Parent?.CastSafeTo<TOptions>()));
        }

        protected List<T> GetMerged<T>(params List<T>[] lists)
        {
            List<T> merged = new();
            foreach (List<T> list in lists.Where(x => x != null))
            {
                list.Where(item => !merged.Contains(item)).ForEach(item => merged.Add(item));
            }
            return merged;
        }

        protected Dictionary<TKey, TValue> GetMerged<TOptions, TKey, TValue>(Dictionary<TKey, TValue> current, Func<TOptions, Dictionary<TKey, TValue>> getAction)
            where TOptions : class
        {
            return this.GetMerged(current, getAction(this.Caller?.CastSafeTo<TOptions>()), getAction(this.Parent?.CastSafeTo<TOptions>()));
        }

        protected Dictionary<TKey, TValue> GetMerged<TKey, TValue>(params Dictionary<TKey, TValue>[] dictionaries)
        {
            Dictionary<TKey, TValue> merged = new();
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries.Where(x => x != null))
            {
                dictionary.Where(pair => !merged.ContainsKey(pair.Key)).ForEach(pair => merged.Add(pair.Key, pair.Value));
            }
            return merged;
        }

        public void Add(object part)
        {
            this.Parts.Add(part.GetType(), part);
        }

        public T Get<T>()
            where T : class
        {
            return this.Parts.ContainsKey(typeof(T)) ? (T)this.Parts[typeof(T)] : default;
        }

        public T GetOrRead<T>(Func<Options, T> readAction)
        {
            if (!this.Parts.ContainsKey(typeof(T)))
            {
                this.Parts.Add(typeof(T), readAction(this));
            }
            return (T)this.Parts[typeof(T)];
        }
    }
}
