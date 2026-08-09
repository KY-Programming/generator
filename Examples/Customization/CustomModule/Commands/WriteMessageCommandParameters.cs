using KY.Generator.Command;

namespace CustomModule.Commands;

/// <summary>
/// Everything one run of the command needs. Parameters live in this half because the fluent API builds
/// them, and the fluent API is what a user of the module references.
/// </summary>
public class WriteMessageCommandParameters : GeneratorCommandParameters
{
    /// <summary>
    /// The names the command can be called by on the command line. ToCommand turns the class name into
    /// "write-message" and "writemessage"; further aliases can be added after it.
    /// </summary>
    public static string[] Names { get; } = [..ToCommand(nameof(WriteMessageCommandParameters))];

    /// <summary>The text the generated class writes to the console.</summary>
    public string? Message { get; set; }

    /// <summary>Name of the generated class. The generated file is named after it.</summary>
    public string? ClassName { get; set; }

    /// <summary>Namespace of the generated class.</summary>
    public string? Namespace { get; set; }

    /// <summary>Creates the parameters with the first of the <see cref="Names"/> as command name.</summary>
    public WriteMessageCommandParameters()
        : base(Names.First())
    { }
}
