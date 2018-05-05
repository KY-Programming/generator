namespace KY.Generator.Templates
{
    public class InlineIfTemplate : CodeFragment
    {
        public CodeFragment Condition { get; }
        public CodeFragment TrueFragment { get; }
        public CodeFragment FalseFragment { get; }

        public InlineIfTemplate(CodeFragment condition, CodeFragment trueFragment, CodeFragment falseFragment)
        {
            this.Condition = condition;
            this.TrueFragment = trueFragment;
            this.FalseFragment = falseFragment;
        }
    }
}
