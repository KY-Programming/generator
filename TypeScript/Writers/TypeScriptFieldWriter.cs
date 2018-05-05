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
    public class TypeScriptFieldWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public TypeScriptFieldWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            FieldTemplate template = (FieldTemplate)fragment;
            MetaStatement metaStatement = elements.AddClosed();
            metaStatement.Code.WithSeparator(" ", x => x.Add(template.Visibility == Visibility.None ? string.Empty : template.Visibility.ToString().ToLower())
                                                        .Add(template.IsStatic ? "static" : string.Empty)
                                                        .Add(template.IsConst ? "const" : string.Empty)
                                                        .Add(template.Name)
                                                        .Add(":")
                                                        .Add(template.Type, this.Language));
            if (template.DefaultValue != null)
            {
                metaStatement.Code.Add(" = ").Add(template.DefaultValue, this.Language);
            }
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}