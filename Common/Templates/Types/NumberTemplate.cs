using System;

namespace KY.Generator.Templates
{
    public class NumberTemplate : ChainedCodeFragment, ICloneable
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

        object ICloneable.Clone()
        {
            if (this.LongValue != null)
            {
                return new NumberTemplate(this.LongValue.Value);
            }
            if (this.FloatValue != null)
            {
                return new NumberTemplate(this.FloatValue.Value);
            }
            if (this.DoubleValue != null)
            {
                return new NumberTemplate(this.DoubleValue.Value);
            }
            throw new InvalidOperationException();
        }
    }
}
