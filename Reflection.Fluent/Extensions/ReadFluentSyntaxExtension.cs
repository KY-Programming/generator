using KY.Core;

namespace KY.Generator;

public static class ReadFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Reflection read commands. Use at least one command!
    /// </summary>
    public static IReadFluentSyntax Reflection(this IReadFluentSyntax syntax, Action<IReflectionReadSyntax> action)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        IReflectionReadSyntax readSyntax = internalSyntax.Resolver.Get<ISyntaxResolver>().Create<IReflectionReadSyntax>();
        IExecutableSyntax executableSyntax = readSyntax.CastTo<IExecutableSyntax>();
        internalSyntax.Syntaxes.Add(executableSyntax);
        action(readSyntax);
        executableSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Reflection)} action requires at least one command. E.g. '.{nameof(Reflection)}(read => read.{nameof(IReflectionReadSyntax.FromType)}<MyModel>())'");
        return internalSyntax;
    }
}
