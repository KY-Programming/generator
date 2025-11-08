namespace KY.Generator.Templates;

public class BaseTemplate : ChainedCodeFragment
{
    public override string Separator => " ";

    public override object Clone()
    {
        return this.CloneTo(new BaseTemplate());
    }
}
