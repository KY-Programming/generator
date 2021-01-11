namespace KY.Generator.Templates
{
    public class BooleanTemplate : ChainedCodeFragment
    {
        public override string Separator => " ";
        public bool Value { get; }

        public BooleanTemplate(bool value)
        {
            this.Value = value;
        }
    }
}