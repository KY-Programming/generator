namespace KY.Generator;

public interface ITypeScriptModelSyntax : IFluentSyntax
{
    /// <summary>
    /// If set to false, stops formatting the language specific naming like from C# property <code>MyProperty</code> to TypeScript property <code>myProperty</code>
    /// </summary>
    ITypeScriptModelSyntax FormatNames(bool value = true);

    /// <summary>
    /// Sets the output path. Can be absolute or relative to the .csproj or the global output if a global output is set
    /// </summary>
    /// <param name="path">Absolute or relative to the .csproj or the global output if a global output is set</param>
    ITypeScriptModelSyntax OutputPath(string path);

    /// <summary>
    /// Skips the generation of namespace e.g. for TypeScript classes. Default TRUE
    /// </summary>
    ITypeScriptModelSyntax SkipNamespace(bool value = true);

    /// <summary>
    /// Tries to generate all models as interfaces
    /// </summary>
    ITypeScriptModelSyntax PreferInterfaces();
}
