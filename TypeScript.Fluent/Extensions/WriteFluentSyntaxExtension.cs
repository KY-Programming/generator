namespace KY.Generator;

public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Generates code that is not restricted by TypeScripts strict mode. By default the generated code is strict.
    /// </summary>
    /// <param name="value">Set to false to switch back to strict</param>
    public static IWriteFluentSyntax NonStrict(this IWriteFluentSyntax syntax, bool value = true)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        // The syntax has to run on the resolver of the fluent chain - a new command scope would come with its own
        // Options and the option set here would never reach the commands.
        ITypeScriptSyntax typeScriptSyntax = internalSyntax.Resolver.Get<ISyntaxResolver>().Create<ITypeScriptSyntax>(internalSyntax.Resolver);
        typeScriptSyntax.NonStrict(value);
        return syntax;
    }

    /// <summary>
    /// Does not generate index.ts files anymore
    /// </summary>
    public static IWriteFluentSyntax NoIndex(this IWriteFluentSyntax syntax)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        // The syntax has to run on the resolver of the fluent chain - a new command scope would come with its own
        // Options and the option set here would never reach the commands.
        ITypeScriptSyntax typeScriptSyntax = internalSyntax.Resolver.Get<ISyntaxResolver>().Create<ITypeScriptSyntax>(internalSyntax.Resolver);
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
