using KY.Generator.Command;

namespace KY.Generator.Commands;

public class StatisticsCommandParameters : GeneratorCommandParameters
{
    public string? File { get; set; }

    public static string[] Names { get; } = [..ToCommand(nameof(StatisticsCommand)), "statistic", "stats", "stat"];

    public StatisticsCommandParameters()
        : base(Names.First())
    { }
}
