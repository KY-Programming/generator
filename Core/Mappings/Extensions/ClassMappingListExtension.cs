using System.Collections.Generic;
using System.Linq;

namespace KY.Generator.Mappings
{
    public static class ClassMappingListExtension
    {
        private static ClassMapping GetMapping(this List<ClassMapping> list, string name)
        {
            return list.FirstOrDefault(x => x.Name == name);
        }

        public static bool IsIgnored(this List<ClassMapping> list, string name)
        {
            return list.GetMapping(name)?.Ignored ?? false;
        }

        public static string Get(this List<ClassMapping> list, string name)
        {
            return list.Get(name, name);
        }

        public static string Get(this List<ClassMapping> list, string name, string defaultValue)
        {
            return list.GetMapping(name)?.MappedName ?? defaultValue;
        }
    }
}