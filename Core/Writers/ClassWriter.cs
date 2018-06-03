using System;
using System.Linq;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Core.Meta.Templates;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class ClassWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public ClassWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            ClassTemplate template = (ClassTemplate)fragment;
            elements.AddBlankLine();
            elements.Add(template.Comment, this.Language);
            elements.Add(template.Attributes, this.Language);
            MetaBlock statement = elements.AddBlock();
            MetaStatement header = statement.Header.AddUnclosed();
            header.Code.WithSeparator(" ", x => x
                                                .Add(this.Language.ClassScope)
                                                .Add(template.IsAbstract ? "abstract" : string.Empty)
                                                .Add(template.IsStatic ? "static" : this.Language.PartialKeyword)
                                                .Add(template.IsInterface ? "interface" : "class")
                                                .Add(this.Language.GetClassName(template)));
            if (template.Generics.Count > 0)
            {
                header.Code.Add("<").Add(template.Generics, this.Language, ", ").Add(">");
            }
            header.Code.Add(template.BasedOn, this.Language);
            header.Code.Add(template.Generics.Select(x => x.ToConstraints()), this.Language);
            if (template.IsInterface)
            {
                template.Fields.ForEach(x => x.Visibility = Visibility.None);
                template.Properties.ForEach(x => x.Visibility = Visibility.None);
            }
            if (template.Classes.Count > 0)
            {
                statement.Elements.Add(template.Classes, this.Language);
            }
            if (template.Fields.Count > 0)
            {
                statement.Elements.AddBlankLine();
                statement.Elements.Add(template.Fields, this.Language);
            }
            if (template.Properties.Count > 0)
            {
                statement.Elements.AddBlankLine();
                statement.Elements.Add(template.Properties, this.Language);
            }
            if (template.Code != null)
            {
                statement.Elements.AddBlankLine();
                statement.Elements.AddUnclosed().Code.Add(template.Code, this.Language);
            }
            if (template.Methods.Count > 0)
            {
                statement.Elements.AddBlankLine();
                statement.Elements.Add(template.Methods, this.Language);
            }
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}