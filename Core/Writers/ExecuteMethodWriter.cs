using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class ExecuteMethodWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public ExecuteMethodWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            ExecuteMethodTemplate template = (ExecuteMethodTemplate)fragment;
            fragments.Add(template.Name)
                     .Add("(")
                     .Add(template.Parameters, this.Language, ", ")
                     .Add(")");
        }
    }
}