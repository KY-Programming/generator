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
            output.Add(template.Types.Single())
                  .Add("[]");
        }
    }
}