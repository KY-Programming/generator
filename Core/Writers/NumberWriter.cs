using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NumberWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            NumberTemplate template = (NumberTemplate)fragment;
            fragments.Add(template.Value.ToString());
        }
    }
}