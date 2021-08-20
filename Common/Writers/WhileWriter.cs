using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class WhileWriter : ITemplateWriter
    {
        public void Write(ICodeFragment fragment, IOutputCache output)
        {
            WhileTemplate template = (WhileTemplate)fragment;
            output.Add("while (")
                  .Add(template.Condition)
                  .Add(")")
                  .StartBlock()
                  .Add(template.Code)
                  .EndBlock();
        }
    }
}
