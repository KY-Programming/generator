namespace KY.Generator.Templates
{
    public class CaseTemplate : ICodeFragment
    {
        public ICodeFragment Expression { get; }
        public MultilineCodeFragment Code { get; }

        public CaseTemplate(ICodeFragment expression)
        {
            this.Expression = expression;
            this.Code = new MultilineCodeFragment();
        }
    }
}