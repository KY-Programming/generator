using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptGenericTypeWriter : GenericTypeWriter
    {
        public TypeScriptGenericTypeWriter()
        {
            this.Writers.Add("Array", new TypeScriptArrayTypeWriter());
            this.Writers.Add("Dictionary", new TypeScriptDictionaryWriter());
        }

        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            GenericTypeTemplate template = (GenericTypeTemplate)fragment;
            if (template.Name == "List" || template.Name == "IList" || template.Name == "IEnumerable")
            {
                output.Add(template.Types.Single().Name)
                      .Add("[]");
            }
            else
            {
                base.Write(fragment, output);
            }
        }
    }
}