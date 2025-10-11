namespace KY.Generator.Templates;

public class AppendAssignStringTemplate : ChainedCodeFragment
{
    public ICodeFragment Code { get; }

    public override string Separator => "";

    public AppendAssignStringTemplate(ICodeFragment code)
    {
        this.Code = code;
    }

    public override object Clone()
    {
        return this.CloneTo(new AppendAssignStringTemplate(this.Code));
    }
}
