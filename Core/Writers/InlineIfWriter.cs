using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class InlineIfWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public InlineIfWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            InlineIfTemplate template = (InlineIfTemplate)fragment;
            fragments.Add(template.Condition, this.Language)
                     .Add(" ? ")
                     .Add(template.TrueFragment, this.Language)
                     .Add(" : ")
                     .Add(template.FalseFragment, this.Language);
        }
    }
}