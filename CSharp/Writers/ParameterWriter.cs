using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class ParameterWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ParameterWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            ParameterTemplate template = (ParameterTemplate)fragment;
            fragments.Add(template.Attributes, this.Language);
            fragments.Add(template.Type, this.Language)
                     .Add(" ")
                     .Add(template.Name);
            if (template.DefaultValue != null)
            {
                fragments.Add(" = ")
                         .Add(template.DefaultValue, this.Language);
            }
        }
    }
}