using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    internal class DeclareTypeWriter : ITemplateWriter
    {
        public void Write(ICodeFragment fragment, IOutputCache output)
        {
            DeclareTypeTemplate template = (DeclareTypeTemplate)fragment;
            output.Add("declare type ")
                  .Add(template.Name)
                  .Add(" = ")
                  .Add(template.Type)
                  .BreakLine()
                  .BreakLine();
        }
    }
}