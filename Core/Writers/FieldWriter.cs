using System;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class FieldWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public FieldWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            FieldTemplate template = (FieldTemplate)fragment;
            FieldTemplate lastTemplate = this.Language.LastFragment as FieldTemplate;
            if (template.Attributes.Count > 0 || lastTemplate?.Attributes.Count > 0)
            {
                elements.AddBlankLine();
            }
            elements.Add(template.Attributes, this.Language);
            MetaStatement metaStatement = elements.AddClosed();
            metaStatement.Code.WithSeparator(" ", x => x.Add(template.Visibility == Visibility.None ? string.Empty : template.Visibility.ToString().ToLower())
                                                        .Add(template.IsStatic ? "static" : string.Empty)
                                                        .Add(template.IsConst ? "const" : string.Empty)
                                                        .Add(template.Type, this.Language)
                                                        .Add(this.Language.GetFieldName(template)));
            if (template.DefaultValue != null)
            {
                metaStatement.Code.Add(" = ").Add(template.DefaultValue, this.Language);
            }
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}