namespace KY.Generator.Templates;

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

    public override object Clone()
    {
        if (this.LongValue != null)
        {
            return this.CloneTo(new NumberTemplate(this.LongValue.Value));
        }
        if (this.FloatValue != null)
        {
            return this.CloneTo(new NumberTemplate(this.FloatValue.Value));
        }
        if (this.DoubleValue != null)
        {
            return this.CloneTo(new NumberTemplate(this.DoubleValue.Value));
        }
        throw new NotImplementedException("Cloning of this type of Number is not implemented");
    }
}
