using System.Linq;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

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
                statement.Header.AddUnclosed().Code.AddNewLine()
                         .Add(" : base(")
                         .Add(constructorTemplate.BaseConstructor, this.Language)
                         .Add(")");
            }
            else if (constructorTemplate.ThisConstructor != null)
            {
                statement.Header.AddUnclosed().Code.AddNewLine()
                         .Add(" : this(")
                         .Add(constructorTemplate.ThisConstructor, this.Language)
                         .Add(")");
            }
        }
    }
}