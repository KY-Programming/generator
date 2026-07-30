using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptArrayTypeWriter : ITypeWriter, IGenericTypeWriter
    {
        public void Write(TypeTemplate template, IOutputCache output)
        {
            output.Add("[]");
        }

        public void Write(GenericTypeTemplate template, IOutputCache output)
        {
            TypeTemplate elementType = template.Types.Single();
            // a union as element type has to be parenthesized: (string | undefined)[]
            bool isUnion = elementType is NullableTypeTemplate { Strict: true };
            output.If(isUnion).Add("(").EndIf()
                  .Add(elementType)
                  .If(isUnion).Add(")").EndIf()
                  .Add("[]");
        }
    }
}