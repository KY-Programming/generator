using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    internal class CsharpGenericTypeWriter : GenericTypeWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            GenericTypeTemplate template = (GenericTypeTemplate)fragment;
            if (template.Name == "Nullable")
            {
                output.Add(template.Types.Single().Name).Add("?");
            }
            else
            {
                base.Write(fragment, output);
            }
        }
    }
}