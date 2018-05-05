using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class ThrowWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ThrowWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            ThrowTemplate template = (ThrowTemplate)fragment;
            fragments.Add("throw new ")
                     .Add(template.Type, this.Language)
                     .Add("(")
                     .Add(template.Parameters, this.Language, ", ")
                     .Add(")");
        }
    }
}