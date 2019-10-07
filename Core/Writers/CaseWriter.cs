using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class CaseWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            CaseTemplate template = (CaseTemplate)fragment;
            output.Add("case ").Add(template.Expression).Add(":")
                  .Indent().Add(template.Code)
                  .Add("break").CloseLine()
                  .UnIndent();
        }
    }
}