using KY.Core;

namespace KY.Generator;

public static class ReadFluentSyntaxExtension
{
    /// <summary>
    /// Executes the JSON read commands. Use at least one command!
    /// </summary>
    public static IReadFluentSyntax Json(this IReadFluentSyntax syntax, Action<IJsonReadSyntax> action)
    {
        IReadFluentSyntaxInternal internalSyntax = (IReadFluentSyntaxInternal)syntax;
        JsonReadSyntax readSyntax = new();
        internalSyntax.Syntaxes.Add(readSyntax);
        action(readSyntax);
        readSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(Json)} action requires at least one command. E.g. '.{nameof(Json)}(read => read.{nameof(IJsonReadSyntax.FromFile)}(\"my\\path\"))'");
        return internalSyntax;
    }
}
