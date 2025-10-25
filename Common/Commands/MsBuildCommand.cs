using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class MsBuildCommand : GeneratorCommand<GeneratorCommandParameters>, IPrepareCommand
{
    private readonly IEnvironment environment;
    private readonly StatisticsService statisticsService;
    public static string[] Names { get; } = [..ToCommand(nameof(MsBuildCommand)), "msbuild"];

    public MsBuildCommand(IEnvironment environment, StatisticsService statisticsService)
    {
        this.environment = environment;
        this.statisticsService = statisticsService;
    }

    public override IGeneratorCommandResult Run()
    {
        this.environment.IsMsBuild = true;
        this.statisticsService.Data.IsMsBuild = true;
        return this.Success();
    }
}
