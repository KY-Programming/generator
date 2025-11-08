using KY.Core;

namespace KY.Generator;

public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Reflection write commands. Use at least one command!
    /// </summary>
    public static IWriteFluentSyntax Reflection(this IWriteFluentSyntax syntax, Action<IReflectionWriteSyntax> action)
    {
        IWriteFluentSyntaxInternal internalSyntax = (IWriteFluentSyntaxInternal)syntax;
        IReflectionWriteSyntax writeSyntax = internalSyntax.Resolver.Create<IReflectionWriteSyntax>();
        IExecutableSyntax executableSyntax = writeSyntax.CastTo<IExecutableSyntax>();
        internalSyntax.Syntaxes.Add(executableSyntax);
        action(writeSyntax);
        executableSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Reflection)} action requires at least one command. E.g. '.{nameof(Reflection)}(write => write.{nameof(IReflectionWriteSyntax.Models)}(...))'");
        return internalSyntax;
    }
}
