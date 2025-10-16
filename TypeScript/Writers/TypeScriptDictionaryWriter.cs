using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers;

public class TypeScriptDictionaryWriter : IGenericTypeWriter
{
    public void Write(GenericTypeTemplate template, IOutputCache output)
    {
        TypeTemplate keyType = template.Types.First();
        TypeTemplate valueType = template.Types.Second();
        if (keyType.Name is "string" or "number")
        {
            output.Add("Record<").Add(keyType).Add(", ").Add(valueType).Add(">");
        }
        else
        {
            output.Add($"{{ /* unsupported type for key. Expected string or number. Got '{keyType.Name}'. */ }}");
        }
    }
}
