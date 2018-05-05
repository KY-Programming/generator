using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    internal class ClassGenericWriter : ITemplateWriter
    {
        public void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddUnclosed().Code, fragment);
        }

        public void Write(IMetaFragmentList fragments, CodeFragment fragment)
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