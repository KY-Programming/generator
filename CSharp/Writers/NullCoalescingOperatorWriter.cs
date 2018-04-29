using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NullCoalescingOperatorWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public NullCoalescingOperatorWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            NullCoalescingOperatorTemplate template = (NullCoalescingOperatorTemplate)fragment;
            fragments.Add(template.LeftCode, this.Language)
                     .Add(" ?? ")
                     .Add(template.RightCode, this.Language);
        }
    }
}