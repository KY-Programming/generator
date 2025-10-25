using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class YieldReturnWriter : ITemplateWriter
    {
        public void Write(ICodeFragment fragment, IOutputCache output)
        {
            YieldReturnTemplate template = (YieldReturnTemplate)fragment;
            output.Add("yield return ")
                  .Add(template.Code)
                  .CloseLine();
        }
    }
}
