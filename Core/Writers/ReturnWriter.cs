using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ReturnWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ReturnWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            ReturnTemplate template = (ReturnTemplate)fragment;
            fragments.Add("return ")
                     .Add(template.Code, this.Language);
        }
    }
}