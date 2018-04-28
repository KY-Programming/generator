using System;
using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Models;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class PropertyWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public PropertyWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            PropertyTemplate template = (PropertyTemplate)fragment;
            PropertyTemplate lastTemplate = this.Language.LastFragment as PropertyTemplate;
            if (template.Attributes.Count > 0 || lastTemplate?.Attributes.Count > 0)
            {
                elements.AddBlankLine();
            }
            elements.Add(template.Comment, this.Language);
            elements.Add(template.Attributes, this.Language);
            MetaStatement statement = elements.AddUnclosed();
            statement.Code.Add(template.Visibility == Visibility.None ? string.Empty : template.Visibility.ToString().ToLower())
                     .Add(" ")
                     .Add(template.IsVirtual ? "virtual " : string.Empty)
                     .Add(template.IsStatic ? "static " : string.Empty)
                     .Add(template.Type, this.Language)
                     .Add(" ")
                     .Add(template.Name);
            if (template.HasGetter || template.HasSetter)
            {
                statement.Code.Add(" { ")
                         .Add(template.HasGetter ? "get; " : string.Empty)
                         .Add(template.HasSetter ? "set; " : string.Empty)
                         .Add("}");
            }
            if (template.DefaultValue != null)
            {
                statement.Closed().Code.Add(" = ")
                         .Add(template.DefaultValue, this.Language);
            }
            if (template.Expression != null)
            {
                statement.Closed().Code.Add(" => ")
                         .Add(template.Expression, this.Language);
            }
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}