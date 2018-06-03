using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;

namespace KY.Generator.Templates.Extensions
{
    public static class MetaFragmentListExtension
    {
        public static IMetaFragmentList Add(this IMetaFragmentList list, ICodeFragment code, ILanguage language)
        {
            language.Write(list, code);
            return list;
        }

        public static IMetaFragmentList Add(this IMetaFragmentList list, IEnumerable<ICodeFragment> code, ILanguage language, string separator)
        {
            List<ICodeFragment> fragments = code.ToList();
            ICodeFragment last = fragments.LastOrDefault();
            foreach (ICodeFragment fragment in fragments)
            {
                list.Add(fragment, language);
                if (!fragment.Equals(last))
                {
                    list.Add(separator);
                }
            }
            return list;
        }

        public static IMetaFragmentList Add(this IMetaFragmentList list, IEnumerable<ICodeFragment> code, ILanguage language)
        {
            code.ForEach(x => list.Add(x, language));
            return list;
        }
    }
}