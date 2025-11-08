using KY.Core;

namespace KY.Generator;

public static class ReadFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Sqlite read commands. Use at least one command!
    /// </summary>
    public static IReadFluentSyntax Sqlite(this IReadFluentSyntax syntax, Action<ISqliteReadSyntax> action)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        ISqliteReadSyntax readSyntax = internalSyntax.Resolver.Create<ISqliteReadSyntax>();
        IExecutableSyntax executableSyntax = readSyntax.CastTo<IExecutableSyntax>();
        internalSyntax.Syntaxes.Add(executableSyntax);
        action(readSyntax);
        executableSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Sqlite)} action requires at least one command. E.g. '.{nameof(Sqlite)}(read => read.{nameof(ISqliteReadSyntax.UseConnectionString)}(...))'");
        return internalSyntax;
    }
}
