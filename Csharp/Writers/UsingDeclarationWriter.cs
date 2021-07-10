using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class UsingDeclarationWriter : ITemplateWriter
    {
        public void Write(ICodeFragment fragment, IOutputCache output)
        {
            UsingDeclarationTemplate template = (UsingDeclarationTemplate)fragment;
            output.Add("using ")
                  .Add(template.Code);
        }
    }
}
