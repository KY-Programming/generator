using System;
using System.Linq;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptMethodWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public TypeScriptMethodWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            MethodTemplate template = (MethodTemplate)fragment;
            elements.AddBlankLine();
            elements.Add(template.Comment, this.Language);
            elements.Add(template.Attributes, this.Language);
            MetaBlock statement = elements.AddBlock();
            IMetaFragmentList headerCode = statement.Header.AddUnclosed().Code;
            headerCode.WithSeparator(" ", x => x.Add(template.Visibility.ToString().ToLower())
                                                .Add(template.IsStatic ? "static" : string.Empty)
                                                .Add(template.IsOverride ? "override" : string.Empty)
                                                .Add(template.Name)
                      )
                      .Add("(")
                      .Add(template.Parameters.OrderBy(x => x.DefaultValue == null ? 0 : 1), this.Language, ", ")
                      .Add(")");
            if (template.Type != null)
            {
                headerCode.Add(": ")
                          .Add(template.Type, this.Language);
            }

            statement.Elements.Add(template.Code, this.Language);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}