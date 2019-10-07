using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class AccessIndexWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AccessIndexTemplate template = (AccessIndexTemplate)fragment;
            output.Add("[").Add(template.Code).Add("]");
        }
    }
}