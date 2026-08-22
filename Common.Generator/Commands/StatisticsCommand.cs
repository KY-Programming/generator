using KY.Generator.Command;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class StatisticsCommand(GlobalStatisticsService globalStatisticsService, StatisticsService statisticsService, SettingsService settingsService)
    : GeneratorCommand<StatisticsCommandParameters>
{
    public override Task<IGeneratorCommandResult> Run()
    {
        if (this.Parameters.File == null)
        {
            return this.SuccessAsync();
        }
        Statistic? statistic = statisticsService.Read(this.Parameters.File);
        if (statistic == null)
        {
            return this.SuccessAsync();
        }
        globalStatisticsService.Read();
        globalStatisticsService.Append(statistic);
        globalStatisticsService.Write();
        if (settingsService.Statistics)
        {
            statisticsService.Anonymize(statistic);
            statisticsService.Submit(statistic);
        }
        statisticsService.Delete(this.Parameters.File);
        return this.SuccessAsync();
    }
}
