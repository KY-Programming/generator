using System.Linq;
using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    internal class ConstructorWriter : MethodWriter
    {
        public ConstructorWriter(BaseLanguage language)
            : base(language)
        { }

        public override void Write(IMetaElementList elements, CodeFragment fragment)
        {
            base.Write(elements, fragment);

            ConstructorTemplate constructorTemplate = (ConstructorTemplate)fragment;
            MetaBlock statement = elements.OfType<MetaBlock>().Last();
            if (constructorTemplate.BaseConstructor != null)
            {
                statement.Header.AddUnclosed().Code.AddNewLine().Add(constructorTemplate.BaseConstructor, this.Language);
            }
            else if (constructorTemplate.ThisConstructor != null)
            {
                statement.Header.AddUnclosed().Code.AddNewLine().Add(constructorTemplate.ThisConstructor, this.Language);
            }
        }
    }
}