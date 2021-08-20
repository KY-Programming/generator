namespace KY.Generator.Templates
{
    public class NumberTemplate : ChainedCodeFragment
    {
        public override string Separator => " ";
        public long? LongValue { get; }
        public float? FloatValue { get; }
        public double? DoubleValue { get; }

        public NumberTemplate(long value)
        {
            this.LongValue = value;
        }

        public NumberTemplate(float value)
        {
            this.FloatValue = value;
        }

        public NumberTemplate(double value)
        {
            this.DoubleValue = value;
        }
    }
}