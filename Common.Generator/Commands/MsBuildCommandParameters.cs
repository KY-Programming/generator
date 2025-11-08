using KY.Generator.Command;

namespace KY.Generator.Commands;

public class MsBuildCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(MsBuildCommand))];
    
    public MsBuildCommandParameters() 
        : base(Names.First())
    { }
}
