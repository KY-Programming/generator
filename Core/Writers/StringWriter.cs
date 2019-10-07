using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class StringWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            StringTemplate template = (StringTemplate)fragment;
            output.Add($"\"{template.Value}\"");
        }
    }
}