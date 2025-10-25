using KY.Generator.Command;

namespace KY.Generator.AspDotNet.Commands;

public class AspDotNetReadHubCommandParameters : GeneratorCommandParameters
{
    public string? Namespace { get; set; }
    public string? Name { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(AspDotNetReadHubCommand)), "asp-read-hub"];

    public AspDotNetReadHubCommandParameters()
        : base(Names.First())
    { }
}
