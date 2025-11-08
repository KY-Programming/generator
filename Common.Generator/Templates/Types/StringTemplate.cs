namespace KY.Generator.Templates;

public class StringTemplate : ChainedCodeFragment
{
    public override string Separator => " ";
    public string Value { get; }

    public StringTemplate(string value)
    {
        this.Value = value;
    }

    public override object Clone()
    {
        return this.CloneTo(new StringTemplate(this.Value));
    }
}
