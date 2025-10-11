namespace KY.Generator.Templates;

public class AsTemplate : ChainedCodeFragment
{
    public override string Separator => " ";
    public TypeTemplate Type { get; }

    public AsTemplate(TypeTemplate type)
    {
        this.Type = type;
    }

    public override object Clone()
    {
        return this.CloneTo(new AsTemplate(this.Type));
    }
}
