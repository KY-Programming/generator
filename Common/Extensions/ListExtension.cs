using System.Collections.Generic;
using KY.Core;

namespace KY.Generator.Extensions
{
    public static class ListExtension
    {
        public static void AddIfNotExists<T>(this IList<T> list, IEnumerable<T> items)
        {
            items.ForEach(list.AddIfNotExists);
        }

        public static void AddIfNotExists<T>(this IList<T> list, T item)
        {
            if (list.Contains(item))
            {
                return;
            }
            list.Add(item);
        }
    }
}
