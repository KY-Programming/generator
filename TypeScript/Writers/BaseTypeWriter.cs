using KY.Generator.Languages;
using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class BaseTypeWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public BaseTypeWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            BaseTypeTemplate baseType = (BaseTypeTemplate)fragment;
            if (baseType == null)
            {
                return;
            }
            TypeTemplate type = baseType.ToType();
            fragments.Add(type.IsInterface ? " implements " : " extends ")
                     .Add(type, this.Language);
        }
    }
}