namespace KY.Generator.Templates;

public class NullConditionalTemplate : ChainedCodeFragment
{
    public override string Separator => "";

    public override object Clone()
    {
        return this.CloneTo(new NullConditionalTemplate());
    }
}
