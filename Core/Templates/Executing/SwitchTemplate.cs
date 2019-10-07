using System.Collections.Generic;

namespace KY.Generator.Templates
{
    public class SwitchTemplate : ICodeFragment
    {
        public ICodeFragment Expression { get; }
        public List<CaseTemplate> Cases { get; }
        public MultilineCodeFragment Default { get; }

        public SwitchTemplate(ICodeFragment expression)
        {
            this.Expression = expression;
            this.Cases = new List<CaseTemplate>();
            this.Default = new MultilineCodeFragment();
        }

        public CaseTemplate AddCase(ICodeFragment expression)
        {
            CaseTemplate caseTemplate = new CaseTemplate(expression);
            this.Cases.Add(caseTemplate);
            return caseTemplate;
        }
    }
}