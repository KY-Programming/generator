using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers;

public class AwaitWriter : ITemplateWriter
{
    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    {
        output.Add("await");
    }
}
