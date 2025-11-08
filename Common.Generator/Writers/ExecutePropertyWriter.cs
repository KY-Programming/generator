using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ExecutePropertyWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ExecutePropertyTemplate template = (ExecutePropertyTemplate)fragment;
            output.Add(template.Name);
        }
    }
}