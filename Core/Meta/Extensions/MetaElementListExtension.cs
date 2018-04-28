using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Meta.Extensions
{
    public static class MetaElementListExtension
    {
        public static MetaStatement AddClosed(this IList<MetaElement> elements)
        {
            MetaStatement statement = new MetaStatement();
            elements.Add(statement);
            return statement;
        }

        public static MetaStatement AddUnclosed(this IList<MetaElement> elements)
        {
            MetaStatement statement = new MetaStatement(false);
            elements.Add(statement);
            return statement;
        }

        public static MetaBlock AddBlock(this IList<MetaElement> elements)
        {
            MetaBlock statement = new MetaBlock();
            elements.Add(statement);
            return statement;
        }

        public static void AddBlankLine(this IList<MetaElement> elements)
        {
            elements.Add(new MetaBlankLine());
        }

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