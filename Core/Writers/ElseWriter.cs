using System;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class ElseWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ElseWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            ElseIfTemplate template = (ElseIfTemplate)fragment;
            MetaBlock statement = elements.AddBlock();
            statement.Header.AddUnclosed().Code
                     .Add("else");
            statement.Elements.Add(template.Code, this.Language);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}