using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    internal class ClassGenericWriter : ITemplateWriter
    {
        public void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddUnclosed().Code, fragment);
        }

        public void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            ClassGenericTemplate template = (ClassGenericTemplate)fragment;
            if (template.Constraints.Count == 0)
            {
                return;
            }
            fragments.Add(template.Name);
        }
    }
}