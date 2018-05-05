using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class TypeScriptOperatorWriter : OperatorWriter
    {
        public override void Write(IMetaFragmentList fragments, CodeFragment fragment)
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