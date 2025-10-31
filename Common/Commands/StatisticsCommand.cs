using KY.Generator.Command;
using KY.Generator.Settings;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class StatisticsCommand : GeneratorCommand<StatisticsCommandParameters>
{
    private readonly GlobalStatisticsService globalStatisticsService;
    private readonly StatisticsService statisticsService;
    private readonly GlobalSettingsService globalSettingsService;

    public StatisticsCommand(GlobalStatisticsService globalStatisticsService, StatisticsService statisticsService, GlobalSettingsService globalSettingsService)
    {
        this.globalStatisticsService = globalStatisticsService;
        this.statisticsService = statisticsService;
        this.globalSettingsService = globalSettingsService;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        if (this.Parameters.File == null)
        {
            return this.SuccessAsync();
        }
        Statistic? statistic = this.statisticsService.Read(this.Parameters.File);
        if (statistic == null)
        {
            return this.SuccessAsync();
        }
        this.globalStatisticsService.Read();
        this.globalStatisticsService.Append(statistic);
        this.globalStatisticsService.Write();
        if (this.globalSettingsService.Read().StatisticsEnabled)
        {
            this.statisticsService.Anonymize(statistic);
            this.statisticsService.Submit(statistic);
        }
        this.statisticsService.Delete(this.Parameters.File);
        return this.SuccessAsync();
    }
}
