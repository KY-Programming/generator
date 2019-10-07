using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class NewWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            NewTemplate template = (NewTemplate)fragment;
            output.Add("new ")
                  .Add(template.Type)
                  .Add("(")
                  .Add(template.Parameters, ", ")
                  .Add(")");
        }
    }
}