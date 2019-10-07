using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ExecuteFieldWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ExecuteFieldTemplate template = (ExecuteFieldTemplate)fragment;
            output.Add(template.Name);
        }
    }
}