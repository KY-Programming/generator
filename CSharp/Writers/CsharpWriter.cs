using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Csharp.Templates;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class CsharpWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddUnclosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            CsharpTemplate template = (CsharpTemplate)fragment;
            fragments.Add(template.Code);
        }
    }
}