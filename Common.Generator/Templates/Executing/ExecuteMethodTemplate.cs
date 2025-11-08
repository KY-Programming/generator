namespace KY.Generator.Templates;

public class ExecuteMethodTemplate : ChainedCodeFragment
{
    public override string Separator => ".";
    public string Name { get; }
    public List<ICodeFragment> Parameters { get; }

    public ExecuteMethodTemplate(string name, params ICodeFragment[] parameters)
        : this(name, (IEnumerable<ICodeFragment>)parameters)
    { }

    public ExecuteMethodTemplate(string name, IEnumerable<ICodeFragment> parameters)
    {
        this.Name = name;
        this.Parameters = new List<ICodeFragment>(parameters);
    }

    public override object Clone()
    {
        return this.CloneTo(new ExecuteMethodTemplate(this.Name, this.Parameters));
    }
}
