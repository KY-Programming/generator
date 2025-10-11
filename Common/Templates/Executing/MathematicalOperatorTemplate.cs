namespace KY.Generator.Templates;

public class MathematicalOperatorTemplate : ChainedCodeFragment
{
    public override string Separator => " ";
    public string OperatorName { get; }

    public MathematicalOperatorTemplate(string operatorName)
    {
        this.OperatorName = operatorName;
    }

    public override object Clone()
    {
        return this.CloneTo(new MathematicalOperatorTemplate(this.OperatorName));
    }
}
