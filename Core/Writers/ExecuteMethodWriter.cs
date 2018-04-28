using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ExecuteMethodWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ExecuteMethodWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            ExecuteMethodTemplate template = (ExecuteMethodTemplate)fragment;
            fragments.Add(template.Name)
                     .Add("(")
                     .Add(template.Parameters, this.Language, ", ")
                     .Add(")");
        }
    }
}