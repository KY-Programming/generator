using KY.Core;
using KY.Generator.Syntax;

namespace KY.Generator;

public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Reflection write commands. Use at least one command!
    /// </summary>
    public static IWriteFluentSyntax Reflection(this IWriteFluentSyntax syntax, Action<IReflectionWriteSyntax> action)
    {
        IWriteFluentSyntaxInternal internalSyntax = (IWriteFluentSyntaxInternal)syntax;
        ReflectionWriteSyntax writeSyntax = new(internalSyntax);
        internalSyntax.Syntaxes.Add(writeSyntax);
        action(writeSyntax);
        writeSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Reflection)} action requires at least one command. E.g. '.{nameof(Reflection)}(write => write.{nameof(IReflectionWriteSyntax.Models)}(...))'");
        return internalSyntax;
    }
}
