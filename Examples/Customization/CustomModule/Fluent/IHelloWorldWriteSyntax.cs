using CustomModule.Commands;
using KY.Generator.Command;

namespace KY.Generator;

/// <summary>
/// The fluent entry point of this module:
/// <code>
/// this.Write(write => write.HelloWorld(hello => hello.Message("Hello World!", "Greeter")));
/// </code>
/// </summary>
public interface IHelloWorldWriteSyntax
{
    /// <summary>
    /// Generates a class that writes <paramref name="message"/> to the console.
    /// </summary>
    IHelloWorldWriteSyntax Message(string message, string className, string? nameSpace = null, string? relativePath = null);
}

/// <summary>
/// The implementation only collects command parameters - it never generates anything itself. That is why
/// it can live in this half and needs nothing from the SDK: the generator picks the commands up and runs
/// them in its own process.
/// </summary>
public class HelloWorldWriteSyntax : IExecutableSyntax, IHelloWorldWriteSyntax
{
    /// <summary>The commands collected so far, run by the generator after the configuration is read.</summary>
    public List<GeneratorCommandParameters> Commands { get; } = [];

    /// <inheritdoc />
    public IHelloWorldWriteSyntax Message(string message, string className, string? nameSpace = null, string? relativePath = null)
    {
        this.Commands.Add(new WriteMessageCommandParameters
                          {
                              Message = message,
                              ClassName = className,
                              Namespace = nameSpace,
                              RelativePath = relativePath
                          });
        return this;
    }
}
