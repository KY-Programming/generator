using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ElseIfWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ElseIfTemplate template = (ElseIfTemplate)fragment;
            output.Add("else if (")
                  .Add(template.Condition)
                  .Add(")");
            output.StartBlock().Add(template.Code).EndBlock();
        }
    }
}