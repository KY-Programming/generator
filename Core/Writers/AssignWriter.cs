using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class AssignWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public AssignWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            AssignTemplate template = (AssignTemplate)fragment;
            fragments.Add("= ")
                     .Add(template.Code, this.Language);
        }
    }
}