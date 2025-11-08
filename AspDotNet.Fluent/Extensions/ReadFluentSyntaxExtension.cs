using KY.Core;

namespace KY.Generator;

public static class ReadFluentSyntaxExtension
{
    /// <summary>
    /// Executes the ASP.NET read commands. Use at least one command!
    /// </summary>
    public static IReadFluentSyntax AspDotNet(this IReadFluentSyntax syntax, Action<IAspDotNetReadSyntax> action)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        IAspDotNetReadSyntax readSyntax = internalSyntax.Resolver.Get<ISyntaxResolver>().Create<IAspDotNetReadSyntax>();
        IExecutableSyntax executableSyntax = readSyntax.CastTo<IExecutableSyntax>();
        internalSyntax.Syntaxes.Add(executableSyntax);
        action(readSyntax);
        executableSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(AspDotNet)} action requires at least one command. E.g. '.{nameof(AspDotNet)}(read => read.{nameof(IAspDotNetReadSyntax.FromController)}<MyController>())'");
        return internalSyntax;
    }
}
