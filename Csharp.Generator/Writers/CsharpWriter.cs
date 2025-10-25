using KY.Generator.Csharp.Templates;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class CsharpWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            CsharpTemplate template = (CsharpTemplate)fragment;
            output.Add(template.Code);
        }
    }
}