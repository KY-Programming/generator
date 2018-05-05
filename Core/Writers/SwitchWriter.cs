using System;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class SwitchWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public SwitchWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            SwitchTemplate template = (SwitchTemplate)fragment;
            MetaBlock block = elements.AddBlock();
            block.Header.AddUnclosed().Code
                 .Add("switch (")
                 .Add(template.Expression, this.Language)
                 .Add(")");
            foreach (CaseTemplate caseTemplate in template.Cases)
            {
                block.Elements.Add(caseTemplate, this.Language);
            }
            if (template.Default.Fragments.Count > 0)
            {
                MetaBlock defaultBlock = block.Elements.AddBlock().SkipStartAndEnd();
                defaultBlock.Header.AddUnclosed().Code.Add("default:");
                defaultBlock.Elements.Add(template.Default, this.Language);
            }
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}