using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers;

public class TypeScriptUnionTypeWriter : ITemplateWriter
{
    public void Write(ICodeFragment fragment, IOutputCache output)
    {
        TypeScriptUnionTypeTemplate template = (TypeScriptUnionTypeTemplate)fragment;
        bool isFirst = true;
        foreach (TypeTemplate typeTemplate in template.Types)
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                output.Add(" | ");
            }
            output.Add(typeTemplate);
        }
    }
}
