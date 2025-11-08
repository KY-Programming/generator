using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ElseWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ElseTemplate template = (ElseTemplate)fragment;
            output.Add("else").StartBlock().Add(template.Code).EndBlock();
        }
    }
}