using KY.Generator.Command;

namespace KY.Generator;

public class AspDotNetReadControllerCommandParameters : GeneratorCommandParameters
{
    public string? Namespace { get; set; }
    public string? Name { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(AspDotNetReadControllerCommandParameters)), "asp-read-controller"];

    public AspDotNetReadControllerCommandParameters()
        : base(Names.First())
    { }
}
