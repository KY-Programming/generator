using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptOperatorWriter : OperatorWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            OperatorTemplate template = (OperatorTemplate)fragment;
            switch (template.Operator)
            {
                case Operator.Equals:
                    output.Add("===");
                    break;
                case Operator.NotEquals:
                    output.Add("!==");
                    break;
                default:
                    base.Write(fragment, output);
                    break;
            }
        }
    }
}