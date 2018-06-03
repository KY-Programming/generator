using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class LocalVariableWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            LocalVariableTemplate template = (LocalVariableTemplate)fragment;
            fragments.Add(template.Name);
        }
    }
}