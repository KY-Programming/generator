namespace KY.Generator.Templates
{
    public class CaseTemplate : CodeFragment
    {
        public CodeFragment Expression { get; }
        public MultilineCodeFragment Code { get; }

        public CaseTemplate(CodeFragment expression)
        {
            this.Expression = expression;
            this.Code = new MultilineCodeFragment();
        }
    }
}