namespace KY.Generator.Templates;

public enum Operator
{
    Not,
    Equals,
    NotEquals,
    Greater,
    GreaterThan,
    Lower,
    LowerThan,
    And,
    Or
}

public class OperatorTemplate : ChainedCodeFragment
{
    public override string Separator => " ";
    public Operator Operator { get; }

    public OperatorTemplate(Operator @operator)
    {
        this.Operator = @operator;
    }

    public override object Clone()
    {
        return this.CloneTo(new OperatorTemplate(this.Operator));
    }
}
