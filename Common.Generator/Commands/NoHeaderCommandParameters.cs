using KY.Generator.Command;

namespace KY.Generator.Commands;

public class NoHeaderCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(NoHeaderCommand))];
    
    public NoHeaderCommandParameters() 
        : base(Names.First())
    { }
}
