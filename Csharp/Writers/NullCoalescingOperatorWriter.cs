using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class NullCoalescingOperatorWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            NullCoalescingOperatorTemplate template = (NullCoalescingOperatorTemplate)fragment;
            output.Add(template.LeftCode)
                  .Add(" ?? ")
                  .Add(template.RightCode);
        }
    }
}