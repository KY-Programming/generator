using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class LambdaWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public LambdaWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            LambdaTemplate template = (LambdaTemplate)fragment;
            fragments.Add(template.ParameterName)
                     .Add(" => ")
                     .Add(template.Code, this.Language);
        }
    }
}