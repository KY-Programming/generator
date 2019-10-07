using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class TypeOfWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            TypeOfTemplate template = (TypeOfTemplate)fragment;
            output.Add("typeof(")
                  .Add(template.Type)
                  .Add(")");
        }
    }
}