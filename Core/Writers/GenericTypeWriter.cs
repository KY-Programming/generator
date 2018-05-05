using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class GenericTypeWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public GenericTypeWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            GenericTypeTemplate template = (GenericTypeTemplate)fragment;
            fragments.Add(template.Name)
                     .Add("<")
                     .Add(template.Types, this.Language, ", ")
                     .Add(">");
        }
    }
}