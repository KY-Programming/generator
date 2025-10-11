namespace KY.Generator.Templates;

public class ThisTemplate : ChainedCodeFragment
{
    public override string Separator => " ";

    public override object Clone()
    {
        return this.CloneTo(new ThisTemplate());
    }
}
