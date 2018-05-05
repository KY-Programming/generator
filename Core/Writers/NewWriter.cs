using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

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