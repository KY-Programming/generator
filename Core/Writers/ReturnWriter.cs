using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ReturnWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ReturnTemplate template = (ReturnTemplate)fragment;
            output.Add("return ")
                  .Add(template.Code)
                  .CloseLine();
        }
    }
}