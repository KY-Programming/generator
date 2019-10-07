using System;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class OperatorWriter : ITemplateWriter
    {
        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            OperatorTemplate template = (OperatorTemplate)fragment;
            switch (template.Operator)
            {
                case Operator.Not:
                    output.Add("!");
                    break;
                case Operator.Equals:
                    output.Add("==");
                    break;
                case Operator.NotEquals:
                    output.Add("!=");
                    break;
                case Operator.Greater:
                    output.Add(">");
                    break;
                case Operator.GreaterThan:
                    output.Add(">=");
                    break;
                case Operator.Lower:
                    output.Add("<");
                    break;
                case Operator.LowerThan:
                    output.Add("<=");
                    break;
                case Operator.And:
                    output.Add("&&");
                    break;
                case Operator.Or:
                    output.Add("||");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}