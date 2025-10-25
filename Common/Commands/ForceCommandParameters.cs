using KY.Generator.Command;

namespace KY.Generator.Commands;

public class ForceCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(ForceCommand))];
    
    public ForceCommandParameters() 
        : base(Names.First())
    { }
}
