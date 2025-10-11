namespace KY.Generator.Templates;

public class NotTemplate : ChainedCodeFragment
{
    public override string Separator => " ";

    public override object Clone()
    {
        return this.CloneTo(new NotTemplate());
    }
}
