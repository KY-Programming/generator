using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class UndefinedValueWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            fragments.Add("undefined");
        }
    }
}