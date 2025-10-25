using KY.Generator.Command;

namespace KY.Generator;

public class TsqlReadCommandParameters : GeneratorCommandParameters
{
    public string? ConnectionString { get; set; }
    public string? Schema { get; set; }
    public string? Table { get; set; }
    public string? StoredProcedure { get; set; }
    public string? Namespace { get; set; }
    public string? Name { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(TsqlReadCommandParameters))];

    public TsqlReadCommandParameters()
        : base(Names.First())
    { }
}
