using KY.Generator.Command;

namespace KY.Generator.TypeScript.Fluent;

internal class TypeScriptModelSyntax : IExecutableSyntax, ITypeScriptModelSyntax
{
    private readonly TypeScriptModelCommandParameters command = new();

    public List<GeneratorCommandParameters> Commands { get; }

    public TypeScriptModelSyntax()
    {
        this.Commands = [this.command];
    }

    public ITypeScriptModelSyntax FormatNames(bool value = true)
    {
        this.command.FormatNames = value;
        return this;
    }

    public ITypeScriptModelSyntax OutputPath(string path)
    {
        this.command.RelativePath = path;
        return this;
    }

    public ITypeScriptModelSyntax SkipNamespace(bool value = true)
    {
        this.command.SkipNamespace = value;
        return this;
    }

    public ITypeScriptModelSyntax PreferInterfaces()
    {
        this.command.PreferInterfaces = true;
        return this;
    }
}
