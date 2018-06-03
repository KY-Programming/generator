using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Csharp.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class NullCoalescingOperatorWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public NullCoalescingOperatorWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            NullCoalescingOperatorTemplate template = (NullCoalescingOperatorTemplate)fragment;
            fragments.Add(template.LeftCode, this.Language)
                     .Add(" ?? ")
                     .Add(template.RightCode, this.Language);
        }
    }
}