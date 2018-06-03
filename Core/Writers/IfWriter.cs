using System;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class IfWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public IfWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            IfTemplate template = (IfTemplate)fragment;
            MetaBlock statement = elements.AddBlock();
            statement.Header.AddUnclosed().Code
                     .Add("if (")
                     .Add(template.Condition, this.Language)
                     .Add(")");
            statement.Elements.Add(template.Code, this.Language);
            if (template.ElseIf.Count > 0)
            {
                statement.Elements.Add(template.ElseIf, this.Language);
            }
            if (template.Else != null)
            {
                statement.Elements.Add(template.Else, this.Language);
            }
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}