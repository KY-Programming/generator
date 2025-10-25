using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptParameterWriter : ParameterWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            ParameterTemplate template = (ParameterTemplate)fragment;
            output.Add(template.Name)
                  .If(template.IsOptional).Add("?").EndIf()
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
