using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class SwitchTemplate : CodeFragment
    {
        public CodeFragment Expression { get; }
        public List<CaseTemplate> Cases { get; }
        public MultilineCodeFragment Default { get; }

        public SwitchTemplate(CodeFragment expression)
        {
            this.Expression = expression;
            this.Cases = new List<CaseTemplate>();
            this.Default = new MultilineCodeFragment();
        }

        public CaseTemplate AddCase(CodeFragment expression)
        {
            CaseTemplate caseTemplate = new CaseTemplate(expression);
            this.Cases.Add(caseTemplate);
            return caseTemplate;
        }
    }
}