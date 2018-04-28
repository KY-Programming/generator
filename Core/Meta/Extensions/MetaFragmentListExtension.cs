using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Meta.Extensions
{
    public static class MetaFragmentListExtension
    {
        public static IMetaFragmentList Add(this IMetaFragmentList list, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                list.Add(new MetaFragment(code));
            }
            return list;
        }

        public static IMetaFragmentList Add(this IMetaFragmentList list, CodeFragment code, ILanguage language)
        {
            language.Write(list, code);
            return list;
        }

        public static IMetaFragmentList Add(this IMetaFragmentList list, IEnumerable<CodeFragment> code, ILanguage language, string separator)
        {
            return list.WithSeparator(separator, x => x.Add(code, language));
        }

        public static IMetaFragmentList Add(this IMetaFragmentList list, IEnumerable<CodeFragment> code, ILanguage language)
        {
            code.ForEach(x => list.Add(x, language));
            return list;
        }

        public static IMetaFragmentList AddNewLine(this IMetaFragmentList list)
        {
            MetaFragment last = list.LastOrDefault();
            if (last != null)
            {
                last.BreakAfter = true;
            }
            return list;
        }

        public static IMetaFragmentList WithSeparator(this IMetaFragmentList list, string separator, Action<IMetaFragmentList> action)
        {
            action(new MetaSeparatorFragmentList(list, separator));
            return list;
        }
    }
}