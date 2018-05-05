using System.Collections.Generic;
using KY.Core.Meta;
using KY.Generator.Languages;

namespace KY.Generator.Templates.Extensions
{
    public static class MetaElementListExtension
    {
        public static void Add(this IMetaElementList elements, CodeFragment fragment, ILanguage language)
        {
            language.Write(elements, fragment);
        }

        public static void Add<T>(this IMetaElementList elements, IEnumerable<T> fragments, ILanguage language)
            where T : CodeFragment
        {
            language.Write(elements, fragments);
        }
    }
}