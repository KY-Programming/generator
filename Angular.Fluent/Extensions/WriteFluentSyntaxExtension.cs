using KY.Core;

namespace KY.Generator;

public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Angular write commands. Use at least one command!
    /// </summary>
    public static IWriteFluentSyntax Angular(this IWriteFluentSyntax syntax, Action<IAngularWriteSyntax> action)
    {
        IFluentInternalSyntax internalSyntax = syntax.CastTo<IFluentInternalSyntax>();
        IAngularWriteSyntax writeSyntax = internalSyntax.Resolver.Get<ISyntaxResolver>().Create<IAngularWriteSyntax>();
        IExecutableSyntax executableSyntax = writeSyntax.CastTo<IExecutableSyntax>();
        internalSyntax.Syntaxes.Add(executableSyntax);
        action(writeSyntax);
        executableSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Angular)} action requires at least one command. E.g. '.{nameof(Angular)}(write => write.{nameof(IAngularWriteSyntax.Models)}(...))'");
        return syntax;
    }
}
