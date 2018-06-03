using System.Collections.Generic;
using KY.Core.Meta;
using KY.Generator.Languages;

namespace KY.Generator.Templates.Extensions
{
    public static class MetaElementListExtension
    {
        public static void Add(this IMetaElementList elements, ICodeFragment fragment, ILanguage language)
        {
            language.Write(elements, fragment);
        }

        public static void Add<T>(this IMetaElementList elements, IEnumerable<T> fragments, ILanguage language)
            where T : ICodeFragment
        {
            language.Write(elements, fragments);
        }
    }
}