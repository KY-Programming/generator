namespace KY.Generator.Templates;

public class NullValueTemplate : ChainedCodeFragment
{
    public override string Separator => " ";

    public override object Clone()
    {
        return this.CloneTo(new NullValueTemplate());
    }
}
