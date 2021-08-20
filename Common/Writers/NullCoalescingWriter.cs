using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NullCoalescingWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            NullCoalescingTemplate template = (NullCoalescingTemplate)fragment;
            output.Add(" ?? ")
                  .Add(template.Code);
        }
    }
}
