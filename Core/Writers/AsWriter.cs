using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class AsWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public AsWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            AsTemplate template = (AsTemplate)fragment;
            fragments.Add("as ")
                     .Add(template.Type, this.Language);
        }
    }
}