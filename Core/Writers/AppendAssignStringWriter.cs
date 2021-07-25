using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class AppendAssignStringWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AppendAssignStringTemplate template = (AppendAssignStringTemplate)fragment;
            output.Add(" += ").Add(template.Code);
        }
    }
}