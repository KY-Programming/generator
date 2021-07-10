using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class VerbatimStringWriter : StringWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            VerbatimStringTemplate template = (VerbatimStringTemplate)fragment;
            output.Add($"@\"{template.Value}\"", true);
            // base.Write(fragment, output);
        }
    }
}
