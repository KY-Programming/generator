using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Settings;
using KY.Generator.Statistics;

namespace KY.Generator.Commands
{
    internal class StatisticsCommand : GeneratorCommand<StatisticsCommandParameters>
    {
        private readonly GlobalStatisticsService globalStatisticsService;
        private readonly StatisticsService statisticsService;
        private readonly GlobalSettingsService globalSettingsService;
        public override string[] Names { get; } = { "statistics", "statistic", "stats", "stat" };

        public StatisticsCommand(GlobalStatisticsService globalStatisticsService, StatisticsService statisticsService, GlobalSettingsService globalSettingsService)
        {
            this.globalStatisticsService = globalStatisticsService;
            this.statisticsService = statisticsService;
            this.globalSettingsService = globalSettingsService;
        }

        public override IGeneratorCommandResult Run()
        {
            Statistic statistic = this.statisticsService.Read(this.Parameters.File);
            if (statistic == null)
            {
                return this.Success();
            }
            this.globalStatisticsService.Read();
            this.globalStatisticsService.Append(statistic);
            this.globalStatisticsService.Analyze();
            this.globalStatisticsService.Write();
            if (this.globalSettingsService.Read().StatisticsEnabled)
            {
                this.statisticsService.Anonymize(statistic);
                this.statisticsService.Submit(statistic);
            }
            return this.Success();
        }
    }
}
