namespace KY.Generator.Templates;

public class DateTimeTemplate : ChainedCodeFragment
{
    public override string Separator => " ";
    public DateTime Value { get; }

    public DateTimeTemplate(DateTime value)
    {
        this.Value = value;
    }

    public override object Clone()
    {
        return this.CloneTo(new DateTimeTemplate(this.Value));
    }
}
