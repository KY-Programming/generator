using System;
using System.Linq;
using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class TypeScriptMethodWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public TypeScriptMethodWriter(BaseLanguage language)
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
                                               .Add(template.Name)
                     )
                     .Add("(")
                     .Add(template.Parameters.OrderBy(x => x.DefaultValue == null ? 0 : 1), this.Language, ", ")
                     .Add("): ")
                     .Add(template.Type, this.Language);

            statement.Elements.Add(template.Code, this.Language);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}