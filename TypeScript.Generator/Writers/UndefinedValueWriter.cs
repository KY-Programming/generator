using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class UndefinedValueWriter : NullValueWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            if (fragment is ForceNullValueTemplate)
            {
                base.Write(fragment, output);
            }
            else
            {
                output.Add("undefined");
            }
        }
    }
}