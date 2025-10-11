namespace KY.Generator.Templates;

public class AppendStringTemplate : ChainedCodeFragment
{
    public ICodeFragment Code { get; }

    public override string Separator => "";

    public AppendStringTemplate(ICodeFragment code)
    {
        this.Code = code;
    }

    public override object Clone()
    {
        return this.CloneTo(new AppendStringTemplate(this.Code));
    }
}
