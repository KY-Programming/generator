using System.Linq;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Csharp.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    internal class ConstructorWriter : MethodWriter
    {
        public ConstructorWriter(BaseLanguage language)
            : base(language)
        { }

        public override void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            base.Write(elements, fragment);

            ConstructorTemplate constructorTemplate = (ConstructorTemplate)fragment;
            MetaBlock statement = elements.OfType<MetaBlock>().Last();
            if (constructorTemplate.ConstructorCall != null)
            {
                statement.Header.AddUnclosed().Code.AddNewLine()
                         .Add(": ")
                         .Add(constructorTemplate.ConstructorCall, this.Language);
            }
        }
    }
}