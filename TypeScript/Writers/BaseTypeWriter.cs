using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class BaseTypeWriter : ITemplateWriter
    {
        protected ILanguage Language { get; }

        public BaseTypeWriter(ILanguage language)
        {
            this.Language = language;
        }

        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
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