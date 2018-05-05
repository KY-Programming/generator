using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class IfTemplate : CodeFragment
    {
        public CodeFragment Condition { get; }
        public MultilineCodeFragment Code { get; }
        public List<ElseIfTemplate> ElseIf { get; }
        public ElseTemplate Else { get; set; }

        public IfTemplate(CodeFragment condition)
        {
            this.Condition = condition;
            this.Code = new MultilineCodeFragment();
            this.ElseIf = new List<ElseIfTemplate>();
        }
    }
}