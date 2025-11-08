using KY.Core;

namespace KY.Generator;

public static class ReadFluentSyntaxExtension
{
    /// <summary>
    /// Executes the Tsql read commands. Use at least one command!
    /// </summary>
    // TODO: Use action instead to insert connectionString parameter here
    public static IReadFluentSyntax Tsql(this IReadFluentSyntax syntax, string connectionString, Action<ITsqlReadSyntax> action)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        ITsqlReadSyntax readSyntax = internalSyntax.Resolver.Create<ITsqlReadSyntax>(connectionString);
        IExecutableSyntax executableSyntax = readSyntax.CastTo<IExecutableSyntax>();
        internalSyntax.Syntaxes.Add(executableSyntax);
        action(readSyntax);
        executableSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Tsql)} action requires at least one command. E.g. '.{nameof(Tsql)}(read => read.{nameof(ITsqlReadSyntax.FromTable)}(...))'");
        return syntax;
    }
}
