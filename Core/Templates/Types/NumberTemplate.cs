namespace KY.Generator.Templates
{
    public class NumberTemplate : ChainedCodeFragment
    {
        public override string Separator => " ";
        public int Value { get; }

        public NumberTemplate(int value)
        {
            this.Value = value;
        }
    }
}