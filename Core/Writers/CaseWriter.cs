using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class CaseWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public CaseWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            CaseTemplate template = (CaseTemplate)fragment;
            MetaBlock block = elements.AddBlock().SkipStartAndEnd();
            this.Write(block.Header.AddUnclosed().Code, fragment);
            block.Elements.Add(template.Code, this.Language);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            CaseTemplate template = (CaseTemplate)fragment;
            fragments.Add("case ")
                     .Add(template.Expression, this.Language)
                     .Add(":");
        }
    }
}