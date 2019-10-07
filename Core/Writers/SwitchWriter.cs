using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class SwitchWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            SwitchTemplate template = (SwitchTemplate)fragment;
            output.Add("switch (")
                  .Add(template.Expression)
                  .Add(")")
                  .StartBlock();
            foreach (CaseTemplate caseTemplate in template.Cases)
            {
                output.Add(caseTemplate);
            }
            if (template.Default.Fragments.Count > 0)
            {
                output.Add("default:")
                      .Indent()
                      .Add(template.Default)
                      .Add("break").CloseLine()
                      .UnIndent();
            }
            output.EndBlock();
        }
    }
}