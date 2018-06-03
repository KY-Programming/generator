using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddUnclosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            TypeScriptTemplate template = (TypeScriptTemplate)fragment;
            fragments.Add(template.Code);
        }
    }
}