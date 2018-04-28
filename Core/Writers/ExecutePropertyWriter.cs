using KY.Generator.Meta;
using KY.Generator.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ExecutePropertyWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
        {
            ExecutePropertyTemplate template = (ExecutePropertyTemplate)fragment;
            fragments.Add(template.Name);
        }
    }
}