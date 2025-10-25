using KY.Generator.Command;

namespace KY.Generator.Commands;

public class GetLicenseCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(GetLicenseCommand)), "l"];

    public GetLicenseCommandParameters()
        : base(Names.First())
    { }
}
