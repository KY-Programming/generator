using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class DateTimeWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            DateTimeTemplate template = (DateTimeTemplate)fragment;
            output.Add($"new DateTime({template.Value.Ticks})");
        }
    }
}