using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

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
            fragments.Add(template.Name)
                     .Add(": ")
                     .Add(template.Type, this.Language);
            if (template.DefaultValue != null)
            {
                fragments.Add(" = ")
                         .Add(template.DefaultValue, this.Language);
            }
        }
    }
}