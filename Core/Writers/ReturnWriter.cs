using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ReturnWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ReturnTemplate template = (ReturnTemplate)fragment;
            if (template.Code == null)
            {
                output.Add("return").CloseLine();
            }
            else
            {
                output.Add("return ")
                      .Add(template.Code)
                      .CloseLine();
            }
        }
    }
}