namespace KY.Generator.Templates;

public class AssignTemplate : ChainedCodeFragment
{
    public override string Separator => " ";
    public ICodeFragment Code { get; }
    public string Operator { get; }

    public AssignTemplate(ICodeFragment code, string @operator = "")
    {
        this.Code = code;
        this.Operator = @operator;
    }

    public override object Clone()
    {
        return this.CloneTo(new AssignTemplate(this.Code, this.Operator));
    }
}
