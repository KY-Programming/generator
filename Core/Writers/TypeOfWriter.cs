using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.Writers
{
    public class TypeOfWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public TypeOfWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            TypeOfTemplate template = (TypeOfTemplate)fragment;
            fragments.Add("typeof(")
                     .Add(template.Type, this.Language)
                     .Add(")");
        }
    }
}