using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class ReturnWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ReturnWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            ReturnTemplate template = (ReturnTemplate)fragment;
            fragments.Add("return ")
                     .Add(template.Code, this.Language);
        }
    }
}