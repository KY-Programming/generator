using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;

namespace KY.Generator.Models
{
    public class Options : IOptions
    {
        protected static Dictionary<object, OptionsCacheEntry> Cache { get; } = new();
        protected bool? StrictValue { get; set; }
        protected bool? PropertiesToFieldsValue { get; set; }
        protected bool? FieldsToPropertiesValue { get; set; }
        protected bool? PreferInterfacesValue { get; set; }
        protected bool? OptionalFieldsValue { get; set; }
        protected bool? OptionalPropertiesValue { get; set; }
        protected bool? IgnoreValue { get; set; }
        protected Dictionary<string, string> ReplaceNameValue { get; } = new();

        public static IOptions Global { get; } = new Options();
        public ICustomAttributeProvider Target { get; set; }
        public IOptions Parent { get; set; }
        public IOptions Caller { get; set; }

        public bool Strict => this.StrictValue ?? this.Parent?.Strict ?? false;
        public bool PropertiesToFields => this.PropertiesToFieldsValue ?? this.Parent?.PropertiesToFields ?? false;
        public bool FieldsToProperties => this.FieldsToPropertiesValue ?? this.Parent?.FieldsToProperties ?? false;
        public bool PreferInterfaces => this.PreferInterfacesValue ?? this.Parent?.PreferInterfaces ?? false;
        public bool OptionalFields => this.OptionalFieldsValue ?? this.Parent?.OptionalFields ?? false;
        public bool OptionalProperties => this.OptionalPropertiesValue ?? this.Parent?.OptionalProperties ?? false;
        public bool Ignore => this.IgnoreValue ?? this.Parent?.Ignore ?? false;
        public Dictionary<string, string> ReplaceName => this.GetMerged(this.ReplaceNameValue, this.Caller?.ReplaceName, this.Parent?.ReplaceName);

        protected List<T> GetMerged<T>(params List<T>[] lists)
        {
            List<T> merged = new();
            foreach (List<T> list in lists.Where(x => x != null))
            {
                list.Where(item => !merged.Contains(item)).ForEach(item => merged.Add(item));
            }
            return merged;
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
