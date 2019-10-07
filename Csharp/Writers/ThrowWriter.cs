using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class ThrowWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ThrowTemplate template = (ThrowTemplate)fragment;
            output.Add("throw new ")
                  .Add(template.Type)
                  .Add("(")
                  .Add(template.Parameters, ", ")
                  .Add(")")
                  .CloseLine();
        }
    }
}