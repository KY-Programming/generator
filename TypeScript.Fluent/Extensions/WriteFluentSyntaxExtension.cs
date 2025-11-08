namespace KY.Generator;

public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Generates code, valid for TypeScripts strict mode
    /// </summary>
    public static IWriteFluentSyntax Strict(this IWriteFluentSyntax syntax, bool value = true)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        ITypeScriptSyntax typeScriptSyntax = internalSyntax.Resolver.Get<ISyntaxResolver>().Create<ITypeScriptSyntax>();
        typeScriptSyntax.Strict(value);
        return syntax;
    }

    /// <summary>
    /// Does not generate index.ts files anymore
    /// </summary>
    public static IWriteFluentSyntax NoIndex(this IWriteFluentSyntax syntax)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        ITypeScriptSyntax typeScriptSyntax = internalSyntax.Resolver.Get<ISyntaxResolver>().Create<ITypeScriptSyntax>();
        typeScriptSyntax.NoIndex();
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
