using System.Collections.Generic;
using System.Linq;
using KY.Generator.Models;

namespace KY.Generator.Extensions
{
    public static class FileNameReplacerExtension
    {
        public static FileNameReplacer Get(this IReadOnlyList<FileNameReplacer> list, string key)
        {
            return list.FirstOrDefault(item => item.Key == key);
        }

        public static FileNameReplacer SetPattern(this FileNameReplacer replacer, string pattern)
        {
            replacer.Pattern = pattern;
            return replacer;
        }

        public static FileNameReplacer SetReplacement(this FileNameReplacer replacer, string replacement)
        {
            replacer.Replacement = replacement;
            return replacer;
        }
    }
}
