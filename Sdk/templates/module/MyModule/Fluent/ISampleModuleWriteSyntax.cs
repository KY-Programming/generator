using KY.Generator.Command;
using MyModule.Commands;

namespace KY.Generator;

/// <summary>
/// The fluent entry point, the alternative to the [GenerateHello] annotation:
/// <code>
/// this.Read(read => { })
///     .Write(write => write.SampleModule(my => my.Hello("Hello world", "Greeter")));
/// </code>
/// The fluent pipeline is always read then write. This module generates from its parameters alone, so
/// the read step stays empty - give it a read action of its own once it reads something.
/// </summary>
public interface ISampleModuleWriteSyntax
{
    /// <summary>
    /// Generates a class that writes <paramref name="message"/> to the console.
    /// </summary>
    ISampleModuleWriteSyntax Hello(string message, string className, string? nameSpace = null, string? relativePath = null);
}

/// <summary>
/// The implementation collects command parameters - it never generates anything itself. The generator
/// picks the commands up and runs them, which is why this can live in the base half and needs nothing
/// from the SDK.
/// </summary>
public class SampleModuleWriteSyntax : IExecutableSyntax, ISampleModuleWriteSyntax
{
    /// <summary>The commands collected so far. The generator runs them after the configuration is read.</summary>
    public List<GeneratorCommandParameters> Commands { get; } = [];

    /// <inheritdoc />
    public ISampleModuleWriteSyntax Hello(string message, string className, string? nameSpace = null, string? relativePath = null)
    {
        this.Commands.Add(new SayHelloCommandParameters
                          {
                              Message = message,
                              ClassName = className,
                              Namespace = nameSpace,
                              RelativePath = relativePath
                          });
        return this;
    }
}
