using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers;

public class TypeScriptNullableTypeWriter : NullableTypeWriter
{
    public override void Write(ICodeFragment fragment, IOutputCache output)
    {
        NullableTypeTemplate template = (NullableTypeTemplate)fragment;
        output.Add(template.Type)
              .If(template.Strict).Add(" | undefined").EndIf();
    }
}
