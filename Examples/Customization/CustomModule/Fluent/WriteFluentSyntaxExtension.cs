using KY.Core;

// Deliberately in the KY.Generator namespace rather than in CustomModule: that is where IWriteFluentSyntax
// lives, so .HelloWorld(...) shows up in IntelliSense without an extra using - exactly like the actions
// the built-in modules add.
namespace KY.Generator;

/// <summary>
/// Adds the HelloWorld action to the fluent write syntax.
/// </summary>
public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Executes the HelloWorld write commands. Use at least one command!
    /// </summary>
    public static IWriteFluentSyntax HelloWorld(this IWriteFluentSyntax syntax, Action<IHelloWorldWriteSyntax> action)
    {
        IWriteFluentSyntaxInternal internalSyntax = (IWriteFluentSyntaxInternal)syntax;
        HelloWorldWriteSyntax writeSyntax = new();
        internalSyntax.Syntaxes.Add(writeSyntax);
        action(writeSyntax);
        writeSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(HelloWorld)} action requires at least one command. E.g. '.{nameof(HelloWorld)}(write => write.{nameof(IHelloWorldWriteSyntax.Message)}(\"Hello World!\", \"Greeter\"))'");
        return internalSyntax;
    }
}
