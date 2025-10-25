using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class BeforeBuildCommand : GeneratorCommand<BeforeBuildCommandParameters>, IPrepareCommand
{
    private readonly IEnvironment environment;
    private readonly StatisticsService statisticsService;

    public BeforeBuildCommand(IEnvironment environment, StatisticsService statisticsService)
    {
        this.environment = environment;
        this.statisticsService = statisticsService;
    }

    public override IGeneratorCommandResult Run()
    {
        this.environment.IsBeforeBuild = true;
        this.statisticsService.Data.IsBeforeBuild = true;
        return this.Success();
    }
}
