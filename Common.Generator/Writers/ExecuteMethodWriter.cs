using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ExecuteMethodWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ExecuteMethodTemplate template = (ExecuteMethodTemplate)fragment;
            output.Add(template.Name)
                  .Add("(")
                  .Add(template.Parameters, ", ")
                  .Add(")");
        }
    }
}