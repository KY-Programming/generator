using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class AppendStringWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            AppendStringTemplate template = (AppendStringTemplate)fragment;
            output.Add(" + ").Add(template.Code);
        }
    }
}