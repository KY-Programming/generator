using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class BaseWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            output.Add("super");
        }
    }
}