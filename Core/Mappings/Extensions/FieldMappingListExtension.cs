using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Mappings
{
    public static class FieldMappingListExtension
    {
        private static FieldMapping GetMapping(this List<FieldMapping> list, string name)
        {
            return list.FirstOrDefault(x => x.Name == name);
        }

        public static bool IsIgnored(this List<FieldMapping> list, string name)
        {
            return list.GetMapping(name)?.Ignored ?? false;
        }

        public static string Get(this List<FieldMapping> list, string name)
        {
            return list.Get(name, name);
        }

        public static string Get(this List<FieldMapping> list, string name, string defaultValue)
        {
            return list.GetMapping(name)?.MappedName ?? defaultValue;
        }
    }
}