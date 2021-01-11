using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptDateTimeWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            DateTimeTemplate template = (DateTimeTemplate)fragment;
            output.Add($"new Date({template.Value.Ticks})");
        }
    }
}