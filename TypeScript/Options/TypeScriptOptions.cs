namespace KY.Generator.TypeScript;

public class TypeScriptOptions(TypeScriptOptions? parent, TypeScriptOptions? global, object? target = null)
    : OptionsBase<TypeScriptOptions>(parent, global, target)
{
    private bool? strict;
    private bool? isStrictSet;
    private bool? noIndex;
    private bool? forceIndex;
    private string? modelOutput;

    public bool Strict
    {
        get => this.GetValue(x => x.strict);
        set => this.strict = value;
    }

    public bool IsStrictSet
    {
        get => this.GetValue(x => x.isStrictSet);
        set => this.isStrictSet = value;
    }

    public bool NoIndex
    {
        get => this.GetValue(x => x.noIndex);
        set => this.noIndex = value;
    }

    public bool ForceIndex
    {
        get => this.GetValue(x => x.forceIndex);
        set => this.forceIndex = value;
    }

    public string? ModelOutput
    {
        get => this.GetValue(x => x.modelOutput);
        set => this.modelOutput = value;
    }
}
