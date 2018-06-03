using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class UsingWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            UsingTemplate template = (UsingTemplate)fragment;
            string typeName = template.Type;
            if (!typeName.StartsWith("*"))
            {
                typeName = $"{{ {typeName} }}";
            }
            fragments.Add($"import {typeName} from \"{template.Path}\"");
        }
    }
}