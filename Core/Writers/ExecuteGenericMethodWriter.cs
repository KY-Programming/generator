using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ExecuteGenericMethodWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ExecuteGenericMethodTemplate template = (ExecuteGenericMethodTemplate)fragment;
            output.Add(template.Name)
                  .Add("<")
                  .Add(template.Types, ", ")
                  .Add(">(")
                  .Add(template.Parameters, ", ")
                  .Add(")");
        }
    }
}