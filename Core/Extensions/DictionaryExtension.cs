using System.Collections;
using System.Collections.Generic;

namespace KY.Generator.Extensions
{
    public static class DictionaryExtension
    {
        public static void SetNullCoalescing<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary[key] = value ?? (dictionary.ContainsKey(key) ? dictionary[key] : default);
        }
    }
}
