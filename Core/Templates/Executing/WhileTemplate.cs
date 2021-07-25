namespace KY.Generator.Templates
{
    public class WhileTemplate : ICodeFragment
    {
        public ICodeFragment Condition { get; }
        public MultilineCodeFragment Code { get; } = new();

        public WhileTemplate(ICodeFragment condition, ICodeFragment code = null)
        {
            this.Condition = condition;
            this.Code.AddLine(code);
        }
    }
}
