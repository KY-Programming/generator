using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            TypeScriptTemplate template = (TypeScriptTemplate)fragment;
            output.Add(template.Code);
        }
    }
}