namespace KY.Generator.TypeScript.Fluent;

public class TypeScriptSyntax : ITypeScriptSyntax
{
    private readonly Options options;

    public TypeScriptSyntax(Options options)
    {
        this.options = options;
    }

    public void Strict(bool value = true)
    {
        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();
        typeScriptOptions.Strict = value;
    }

    public void NoIndex()
    {
        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();
        typeScriptOptions.NoIndex = true;
    }
}
