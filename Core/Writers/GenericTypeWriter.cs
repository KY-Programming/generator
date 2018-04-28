using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

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