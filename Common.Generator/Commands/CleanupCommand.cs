using KY.Generator.Command;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class CleanupCommand : GeneratorCommand<CleanupCommandParameters>
{
    private readonly StatisticsService statisticsService;

    public CleanupCommand(StatisticsService statisticsService)
    {
        this.statisticsService = statisticsService;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        if (this.Parameters.Logs)
        {
            // TODO: Cleanup logs
        }
        if (this.Parameters.Statistics)
        {
            this.statisticsService.Cleanup();
        }
        return this.SuccessAsync();
    }
}
