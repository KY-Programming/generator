using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class UsingWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, CodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, CodeFragment fragment)
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