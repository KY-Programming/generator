using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NumberWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            NumberTemplate template = (NumberTemplate)fragment;
            output.Add(template.Value.ToString());
        }
    }
}