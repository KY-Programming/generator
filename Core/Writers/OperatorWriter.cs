using System;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class OperatorWriter : ITemplateWriter
    {
        public virtual void Write(IMetaElementList elements, ICodeFragment fragment)
        {
            this.Write(elements.AddClosed().Code, fragment);
        }

        public virtual void Write(IMetaFragmentList fragments, ICodeFragment fragment)
        {
            OperatorTemplate template = (OperatorTemplate)fragment;
            switch (template.Operator)
            {
                case Operator.Not:
                    fragments.Add("!");
                    break;
                case Operator.Equals:
                    fragments.Add("==");
                    break;
                case Operator.NotEquals:
                    fragments.Add("!=");
                    break;
                case Operator.Greater:
                    fragments.Add(">");
                    break;
                case Operator.GreaterThan:
                    fragments.Add(">=");
                    break;
                case Operator.Lower:
                    fragments.Add("<");
                    break;
                case Operator.LowerThan:
                    fragments.Add("<=");
                    break;
                case Operator.And:
                    fragments.Add("&&");
                    break;
                case Operator.Or:
                    fragments.Add("||");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}