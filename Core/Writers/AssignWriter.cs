using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class AssignWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AssignTemplate template = (AssignTemplate)fragment;
            output.Add("= ").Add(template.Code);
        }
    }
}