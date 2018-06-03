using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class ParameterWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ParameterWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
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