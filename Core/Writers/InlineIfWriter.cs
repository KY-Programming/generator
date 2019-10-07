using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class InlineIfWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            InlineIfTemplate template = (InlineIfTemplate)fragment;
            output.Add(template.Condition)
                  .Add(" ? ")
                  .Add(template.TrueFragment)
                  .Add(" : ")
                  .Add(template.FalseFragment);
        }
    }
}