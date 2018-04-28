using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NewWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public NewWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            NewTemplate template = (NewTemplate)fragment;
            fragments.Add("new ")
                     .Add(template.Type, this.Language)
                     .Add("(")
                     .Add(template.Parameters, this.Language, ", ")
                     .Add(")");
        }
    }
}