using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class IfWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            IfTemplate template = (IfTemplate)fragment;
            output.Add("if (")
                  .Add(template.Condition)
                  .Add(")")
                  .StartBlock()
                  .Add(template.Code)
                  .EndBlock()
                  .If(template.ElseIf.Count > 0).Add(template.ElseIf).EndIf()
                  .If(template.Else != null).Add(template.Else).EndIf();
        }
    }
}