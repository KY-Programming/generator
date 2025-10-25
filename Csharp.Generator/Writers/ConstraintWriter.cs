using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class ConstraintWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            ConstraintTemplate template = (ConstraintTemplate)fragment;
            if (template.Types.Count == 0)
            {
                return;
            }
            output.BreakLine()
                  .Indent()
                  .Add("where ")
                  .Add(template.Name)
                  .Add(" : ")
                  .Add(template.Types, ", ")
                  .UnIndent();
        }
    }
}