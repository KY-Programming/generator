namespace KY.Generator.Templates
{
    public class InlineIfTemplate : ICodeFragment
    {
        public ICodeFragment Condition { get; }
        public ICodeFragment TrueFragment { get; }
        public ICodeFragment FalseFragment { get; }

        public InlineIfTemplate(ICodeFragment condition, ICodeFragment trueFragment, ICodeFragment falseFragment)
        {
            this.Condition = condition;
            this.TrueFragment = trueFragment;
            this.FalseFragment = falseFragment;
        }
    }
}
