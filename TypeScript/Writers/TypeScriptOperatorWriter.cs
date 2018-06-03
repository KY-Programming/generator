using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptOperatorWriter : OperatorWriter
    {
        public override void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            OperatorTemplate template = (OperatorTemplate)fragment;
            switch (template.Operator)
            {
                case Operator.Equals:
                    fragments.Add("===");
                    break;
                case Operator.NotEquals:
                    fragments.Add("!==");
                    break;
                default:
                    base.Write(fragments, fragment);
                    break;
            }
        }
    }
}