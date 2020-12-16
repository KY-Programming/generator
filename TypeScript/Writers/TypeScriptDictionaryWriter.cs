using System.Linq;
using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptDictionaryWriter : IGenericTypeWriter
    {
        public void Write(GenericTypeTemplate template, IOutputCache output)
        {
            TypeTemplate keyType = template.Types.First();
            TypeTemplate valueType = template.Types.Second();
            if (keyType.Name == "string" || keyType.Name == "number")
            {
                output.Add($"{{ [key: {keyType.Name}]: {valueType.Name}; }}");
            }
            else
            {
                output.Add($"{{ /* unsupported type for key. Expected string or number. Got '{keyType.Name}'. */ }}");
            }
        }
    }
}