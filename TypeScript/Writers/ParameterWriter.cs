using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class ParameterWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ParameterTemplate template = (ParameterTemplate)fragment;
            output.Add(template.Name)
                  .Add(": ")
                  .Add(template.Type);
            if (template.DefaultValue != null)
            {
                output.Add(" = ")
                      .Add(template.DefaultValue);
            }
        }
    }
}