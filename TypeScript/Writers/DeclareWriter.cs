using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class DeclareWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            DeclareTemplate template = (DeclareTemplate)fragment;
            output.Add(template.IsConstant ? "const " : "let ")
                  .Add(template.Name)
                  .If(template.Type != null && !(template.Code is NewTemplate)).Add(": ").Add(template.Type).EndIf()
                  .Add(" = ")
                  .Add(template.Code)
                  .CloseLine();
        }
    }
}