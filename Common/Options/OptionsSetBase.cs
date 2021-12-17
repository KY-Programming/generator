using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;

namespace KY.Generator
{
    public abstract class OptionsSetBase<TSet, TPart>
        where TSet : OptionsSetBase<TSet, TPart>
        where TPart : class, new()
    {
        public TPart Part { get; } = new();
        public TSet Caller { get; }
        public TSet Global { get; }
        public TSet Parent { get; }
        public object Target { get; }

        protected OptionsSetBase(TSet parent, TSet global, TSet caller = null, object target = null)
        {
            this.Caller = caller;
            this.Global = global;
            this.Parent = parent;
            this.Target = target + " " + index++;
        }

        private static int index;

        protected TValue GetPrimitive<TValue>(Func<TPart, TValue?> getAction, TValue defaultValue = default)
            where TValue : struct
        {
            return this.GetValue(getAction, null) ?? defaultValue;
        }

        protected TValue GetGlobalPrimitive<TValue>(Func<TPart, TValue?> getAction, TValue defaultValue = default)
            where TValue : struct
        {
            return this.GetGlobalValue(getAction, null) ?? defaultValue;
        }

        protected TValue? GetValue<TValue>(Func<TPart, TValue?> getAction, TValue? defaultValue = default)
            where TValue : struct
        {
            return getAction(this.Part)
                   ?? this.Global?.GetValue(getAction)
                   ?? this.Caller?.GetValue(getAction)
                   ?? this.Parent?.GetValue(getAction)
                   ?? defaultValue;
        }

        protected TValue GetValue<TValue>(Func<TPart, TValue> getAction, TValue defaultValue = default)
            where TValue : class
        {
            return getAction(this.Part)
                   ?? this.Global?.GetValue(getAction)
                   ?? this.Global?.GetValue(getAction)
                   ?? this.Caller?.GetValue(getAction)
                   ?? this.Parent?.GetValue(getAction)
                   ?? defaultValue;
        }

        protected TValue? GetGlobalValue<TValue>(Func<TPart, TValue?> getAction, TValue? defaultValue = default)
            where TValue : struct
        {
            return getAction(this.Part)
                   ?? this.Global?.GetGlobalValue(getAction)
                   ?? defaultValue;
        }

        protected TValue GetGlobalValue<TValue>(Func<TPart, TValue> getAction, TValue defaultValue = default)
            where TValue : class
        {
            return getAction(this.Part)
                   ?? this.Global?.GetGlobalValue(getAction)
                   ?? defaultValue;
        }

        protected List<TValue> GetMerged<TValue>(Func<TPart, List<TValue>> getAction)
        {
            return this.GetMerged(getAction(this.Part), this.Global?.GetMerged(getAction), this.Caller?.GetMerged(getAction), this.Parent?.GetMerged(getAction));
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

        protected Dictionary<TKey, TValue> GetMerged<TKey, TValue>(Func<TPart, Dictionary<TKey, TValue>> getAction)
        {
            return this.GetMerged(getAction(this.Part), this.Global?.GetMerged(getAction), this.Caller?.GetMerged(getAction), this.Parent?.GetMerged(getAction));
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
    }
}
