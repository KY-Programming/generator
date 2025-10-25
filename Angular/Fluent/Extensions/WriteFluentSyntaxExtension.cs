using KY.Core;
using KY.Generator.Angular.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace
namespace KY.Generator;

public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Angular write commands. Use at least one command!
    /// </summary>
    public static IWriteFluentSyntax Angular(this IWriteFluentSyntax syntax, Action<IAngularWriteSyntax> action)
    {
        IFluentInternalSyntax internalSyntax = syntax.CastTo<IFluentInternalSyntax>();
        AngularWriteSyntax writeSyntax = new();
        internalSyntax.Syntaxes.Add(writeSyntax);
        action(writeSyntax);
        writeSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Angular)} action requires at least one command. E.g. '.{nameof(Angular)}(write => write.{nameof(IAngularWriteSyntax.Models)}(...))'");
        return syntax;
    }
}
