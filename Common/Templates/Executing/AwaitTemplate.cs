namespace KY.Generator.Templates;

public class AwaitTemplate : ChainedCodeFragment
{
    public override string Separator => " ";

    public override object Clone()
    {
        return this.CloneTo(new AwaitTemplate());
    }
}
