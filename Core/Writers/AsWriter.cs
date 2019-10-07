using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class AsWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AsTemplate template = (AsTemplate)fragment;
            output.Add("as ").Add(template.Type);
        }
    }
}