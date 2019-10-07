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
            output.Add("let ")
                  .Add(template.Name)
                  .Add(" = ")
                  .Add(template.Code)
                  .CloseLine();
        }
    }
}