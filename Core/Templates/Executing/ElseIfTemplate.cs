namespace KY.Generator.Templates
{
    public class ElseIfTemplate : ICodeFragment
    {
        public ICodeFragment Condition { get; }
        public MultilineCodeFragment Code { get; }

        public ElseIfTemplate(ICodeFragment condition)
        {
            this.Condition = condition;
            this.Code = new MultilineCodeFragment();
        }
    }
}