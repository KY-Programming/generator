using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class ParenthesisWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ParenthesisTemplate template = (ParenthesisTemplate)fragment;
            output.Add("(")
                  .Add(template.Code)
                  .Add(")");
        }
    }
}