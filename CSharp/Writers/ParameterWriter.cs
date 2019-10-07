using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class ParameterWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ParameterTemplate template = (ParameterTemplate)fragment;
            output.Add(template.Attributes);
            output.Add(template.Type)
                  .Add(" ")
                  .Add(template.Name);
            if (template.DefaultValue != null)
            {
                output.Add(" = ")
                      .Add(template.DefaultValue);
            }
        }
    }
}