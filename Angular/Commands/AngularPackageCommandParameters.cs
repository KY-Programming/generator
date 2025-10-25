using KY.Generator.Angular.Models;
using KY.Generator.Command;

namespace KY.Generator;

public class AngularPackageCommandParameters : GeneratorCommandParameters
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? CliVersion { get; set; }
    public List<AngularPackageDependsOnParameter> DependsOn { get; set; } = new();
    public bool Build { get; set; }
    public bool Publish { get; set; }
    public bool PublishLocal { get; set; }
    public bool VersionFromNpm { get; set; }
    public IncrementVersion IncrementVersion { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(AngularPackageCommandParameters))];

    public AngularPackageCommandParameters()
        : base(Names.First())
    { }
}
