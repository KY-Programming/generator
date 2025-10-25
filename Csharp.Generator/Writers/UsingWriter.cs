using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class UsingWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            UsingTemplate template = (UsingTemplate)fragment;
            output.Add($"using {template.Namespace}").CloseLine();
        }
    }
}