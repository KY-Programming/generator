using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class BooleanWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            BooleanTemplate template = (BooleanTemplate)fragment;
            output.Add(template.Value.ToString().ToLowerInvariant());
        }
    }
}