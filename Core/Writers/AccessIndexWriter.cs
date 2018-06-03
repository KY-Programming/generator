using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class AccessIndexWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public AccessIndexWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddUnclosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            AccessIndexTemplate template = (AccessIndexTemplate)fragment;
            fragments.Add("[")
                     .Add(template.Code, this.Language)
                     .Add("]");
        }
    }
}