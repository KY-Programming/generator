using KY.Generator.Command;

namespace KY.Generator.Angular.Commands;

public class AngularModelCommandParameters : GeneratorCommandParameters
{
    public static string[] Names { get; } = [..ToCommand(nameof(AngularModelCommand))];
    
    public AngularModelCommandParameters() 
        : base(Names.First())
    { }
}
