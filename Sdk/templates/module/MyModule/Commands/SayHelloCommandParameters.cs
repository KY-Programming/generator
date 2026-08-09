using KY.Generator.Command;

namespace MyModule.Commands;

/// <summary>
/// Everything a single run of the command needs. The parameters live in this half of the module because
/// both entry points build them: the annotation and the fluent API.
/// </summary>
public class SayHelloCommandParameters : GeneratorCommandParameters
{
    /// <summary>
    /// The names this command can be called by on the command line. ToCommand turns the class name into
    /// "say-hello" and "sayhello"; add your own aliases after it.
    /// </summary>
    public static string[] Names { get; } = [..ToCommand(nameof(SayHelloCommandParameters)), "hello"];

    /// <summary>The text the generated class writes to the console.</summary>
    public string? Message { get; set; }

    /// <summary>Name of the generated class. The file is named after it.</summary>
    public string? ClassName { get; set; }

    /// <summary>Namespace of the generated class.</summary>
    public string? Namespace { get; set; }

    /// <summary>Creates the parameters with the first of the <see cref="Names"/> as command name.</summary>
    public SayHelloCommandParameters()
        : base(Names.First())
    { }
}
