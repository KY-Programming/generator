using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class MathWriter : ITemplateWriter
    {
        public void Write(ICodeFragment fragment, IOutputCache output)
        {
            MathematicalOperatorTemplate template = (MathematicalOperatorTemplate)fragment;
            output.Add(template.OperatorName);
        }
    }
}