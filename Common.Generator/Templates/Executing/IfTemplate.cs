using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class IfTemplate : ICodeFragment
    {
        public ICodeFragment Condition { get; }
        public MultilineCodeFragment Code { get; }
        public List<ElseIfTemplate> ElseIf { get; }
        public ElseTemplate Else { get; set; }

        public IfTemplate(ICodeFragment condition)
        {
            this.Condition = condition;
            this.Code = new MultilineCodeFragment();
            this.ElseIf = new List<ElseIfTemplate>();
        }
    }
}