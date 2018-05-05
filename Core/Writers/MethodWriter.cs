using System;
using System.Linq;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class MethodWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public MethodWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            MethodTemplate template = (MethodTemplate)fragment;
            elements.AddBlankLine();
            elements.Add(template.Comment, this.Language);
            elements.Add(template.Attributes, this.Language);
            MetaBlock statement = elements.AddBlock();
            statement.Header.AddUnclosed().Code
                     .WithSeparator(" ", x => x.Add(template.Visibility.ToString().ToLower())
                                               .Add(template.IsStatic ? "static" : string.Empty)
                                               .Add(template.IsOverride ? "override" : string.Empty)
                                               .Add(template.Type, this.Language)
                                               .Add(this.Language.GetMethodName(template))
                     )
                     .Add("(")
                     .Add(template is ExtensionMethodTemplate ? "this " : string.Empty)
                     .Add(template.Parameters.OrderBy(x => x.DefaultValue == null ? 0 : 1), this.Language, ", ")
                     .Add(")");
            
            statement.Elements.Add(template.Code, this.Language);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}