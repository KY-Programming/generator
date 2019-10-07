using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class LocalVariableWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            LocalVariableTemplate template = (LocalVariableTemplate)fragment;
            output.Add(template.Name);
        }
    }
}