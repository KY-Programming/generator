using KY.Core;

// Deliberately in the KY.Generator namespace, not in MyModule: that is where IWriteFluentSyntax lives, so
// the .SampleModule(...) action shows up in IntelliSense without an extra using.
namespace KY.Generator;

/// <summary>
/// Adds the SampleModule action to the fluent write syntax.
/// </summary>
public static class WriteFluentSyntaxExtension
{
    /// <summary>
    /// Executes the SampleModule write commands. Use at least one command!
    /// </summary>
    public static IWriteFluentSyntax SampleModule(this IWriteFluentSyntax syntax, Action<ISampleModuleWriteSyntax> action)
    {
        IWriteFluentSyntaxInternal internalSyntax = (IWriteFluentSyntaxInternal)syntax;
        SampleModuleWriteSyntax writeSyntax = new();
        internalSyntax.Syntaxes.Add(writeSyntax);
        action(writeSyntax);
        writeSyntax.Commands.Count.AssertIsPositive(message: $"The {nameof(SampleModule)} action requires at least one command. E.g. '.{nameof(SampleModule)}(write => write.{nameof(ISampleModuleWriteSyntax.Hello)}(\"Hello world\", \"Greeter\"))'");
        return internalSyntax;
    }
}
