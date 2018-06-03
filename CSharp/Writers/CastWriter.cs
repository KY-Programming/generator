using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class CastWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public CastWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            CastTemplate template = (CastTemplate)fragment;
            fragments.Add("(")
                     .Add(template.Type, this.Language)
                     .Add(")")
                     .Add(template.Code, this.Language);
        }
    }
}