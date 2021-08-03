using System.Collections.Generic;
using KY.Generator.Models;

namespace KY.Generator.Reflection
{
    public static class ReflectionOptionsReader
    {
        public static ReflectionOptionsPart Read(Options entry)
        {
            ReflectionOptionsPart options = new();
            foreach (object attribute in entry.Target.GetCustomAttributes(false))
            {
                switch (attribute)
                {
                    case GenerateIgnoreAttribute:
                        options.Ignore = true;
                        break;
                    case GeneratePreferInterfacesAttribute:
                        options.PreferInterfaces = true;
                        break;
                    case GenerateStrictAttribute:
                        options.Strict = true;
                        break;
                    case GenerateRenameAttribute renameAttribute:
                        Dictionary<string, string> dictionary = options.ReplaceName;
                        if (options.ReplaceName == null)
                        {
                            options.ReplaceName = dictionary = new();
                        }
                        dictionary[renameAttribute.Replace] = renameAttribute.With;
                        options.ReplaceName = dictionary;
                        break;
                }
            }
            return options;
        }
    }
}
