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
            template.Attributes.ForEach(x => x.IsInline = true);
            output.Add(template.Attributes)
                  .Add(template.Type)
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