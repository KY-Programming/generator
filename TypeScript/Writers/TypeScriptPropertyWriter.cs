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
    public class TypeScriptPropertyWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public TypeScriptPropertyWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            PropertyTemplate template = (PropertyTemplate)fragment;
            string fieldName = template.Name == template.Name.ToLower() ? $"_{template.Name}" : template.Name.ToLower();
            MetaStatement field = elements.AddClosed();
            field.Code.Add($"private {fieldName}: ")
                 .Add(template.Type, this.Language);
            if (template.DefaultValue != null)
            {
                field.Code.Add(" = ")
                     .Add(template.DefaultValue, this.Language);
            }
            MetaBlock getter = elements.AddBlock();
            getter.Header.AddUnclosed().Code.WithSeparator(" ", x => x
                                                                     .Add(template.Visibility == Visibility.None ? string.Empty : template.Visibility.ToString().ToLower())
                                                                     .Add(template.IsStatic ? "static" : string.Empty)
                                                                     .Add($"get {template.Name}():")
                                                                     .Add(template.Type, this.Language));
            getter.Elements.AddClosed().Code.Add($"return this.{fieldName}");
            MetaBlock setter = elements.AddBlock();
            setter.Header.AddUnclosed().Code.WithSeparator(" ", x => x
                                                                     .Add(template.Visibility == Visibility.None ? string.Empty : template.Visibility.ToString().ToLower())
                                                                     .Add(template.IsStatic ? "static" : string.Empty)
                                                                     .Add($"set {template.Name}(value:")
                                                                     .Add(template.Type, this.Language)
                                                                     .Add(")"));
            setter.Elements.AddClosed().Code.Add($"this.{fieldName} = value");
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}