using KY.Generator.Syntax;

namespace KY.Generator;

public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Generates code, valid for TypeScripts strict mode
    /// </summary>
    public static IWriteFluentSyntax Strict(this IWriteFluentSyntax syntax, bool value = true)
    {
        TypeScriptOptions options = ((IWriteFluentSyntaxInternal)syntax).Resolver.Get<Options>().Get<TypeScriptOptions>();
        options.Strict = value;
        return syntax;
    }

    /// <summary>
    /// Does not generate index.ts files anymore
    /// </summary>
    public static IWriteFluentSyntax NoIndex(this IWriteFluentSyntax syntax)
    {
        TypeScriptOptions options = ((IWriteFluentSyntaxInternal)syntax).Resolver.Get<Options>().Get<TypeScriptOptions>();
        options.NoIndex = true;
        return syntax;
    }

    /// <summary>
    /// Executes the TypeScript model  write commands
    /// </summary>
    // TODO: Implement TypeScriptModel syntax
    // public static IWriteFluentSyntax TypeScriptModel(this IWriteFluentSyntax syntax)
    // {
    //     IFluentInternalSyntax internalSyntax = syntax.CastTo<IFluentInternalSyntax>();
    //     syntax.co
    //     return syntax;
    // }
}
