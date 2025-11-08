using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class MethodGenericWriter : ITemplateWriter
    {
        public void Write(ICodeFragment fragment, IOutputCache output)
        {
            MethodGenericTemplate template = (MethodGenericTemplate)fragment;
            output.Add(template.Alias)
                  .If(template.DefaultType != null).Add(" = ").Add(template.DefaultType).EndIf();
        }
    }
}