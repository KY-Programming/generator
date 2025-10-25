using KY.Core;
using KY.Generator.Reflection.Fluent;
using KY.Generator.Syntax;

// ReSharper disable once CheckNamespace : Easier usage on lower namespace
namespace KY.Generator;

public static class ReadFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Reflection read commands. Use at least one command!
    /// </summary>
    public static IReadFluentSyntax Reflection(this IReadFluentSyntax syntax, Action<IReflectionReadSyntax> action)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        ReflectionReadSyntax readSyntax = new();
        internalSyntax.Syntaxes.Add(readSyntax);
        action(readSyntax);
        readSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Reflection)} action requires at least one command. E.g. '.{nameof(Reflection)}(read => read.{nameof(IReflectionReadSyntax.FromType)}<MyModel>())'");
        return internalSyntax;
    }
}
