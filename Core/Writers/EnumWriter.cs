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
    public class EnumWriter : ITemplateWriter
    {
        protected BaseLanguage Language { get; }

        public EnumWriter(BaseLanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            EnumTemplate template = (EnumTemplate)fragment;
            elements.AddBlankLine();
            elements.Add(template.Attributes, this.Language);
            MetaBlock statement = elements.AddBlock();
            MetaStatement header = statement.Header.AddUnclosed()
                                            .WithSeparator(" ", x => x
                                                                     .Add(this.Language.ClassScope)
                                                                     .Add("enum")
                                                                     .Add(this.Language.GetClassName(template)));
            header.Code.Add(template.BasedOn, this.Language);
            EnumValueTemplate last = template.Values.LastOrDefault();
            foreach (EnumValueTemplate enumTemplateValue in template.Values)
            {
                statement.Elements.AddUnclosed().Code
                         .Add($"{enumTemplateValue.Name} = ")
                         .Add(enumTemplateValue.Value, this.Language)
                         .Add(last == enumTemplateValue ? string.Empty : ",");
            }
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            throw new InvalidOperationException();
        }
    }
}