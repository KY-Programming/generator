using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class UsingWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            UsingTemplate template = (UsingTemplate)fragment;
            string typeName = template.Type;
            if (!typeName.StartsWith("*"))
            {
                typeName = $"{{ {typeName} }}";
            }
            output.Add($"import {typeName} from \"{template.Path.TrimEnd(".ts")}\"").CloseLine();
        }
    }
}