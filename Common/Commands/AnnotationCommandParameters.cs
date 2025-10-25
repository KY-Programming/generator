using KY.Generator.Command;

namespace KY.Generator.Commands;

public class AnnotationCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(AnnotationCommand)), "attributes"];

    public AnnotationCommandParameters()
        : base(Names.First())
    { }
}
