using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class DeclareWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public DeclareWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            DeclareTemplate template = (DeclareTemplate)fragment;
            fragments.Add(template.Type, this.Language)
                     .Add(" ")
                     .Add(template.Name)
                     .Add(" = ")
                     .Add(template.Code, this.Language);
        }
    }
}