namespace KY.Generator.Templates
{
    public class ElseIfTemplate : CodeFragment
    {
        public CodeFragment Condition { get; }
        public MultilineCodeFragment Code { get; }

        public ElseIfTemplate(CodeFragment condition)
        {
            this.Condition = condition;
            this.Code = new MultilineCodeFragment();
        }
    }
}