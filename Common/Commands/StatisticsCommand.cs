using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Statistics;

namespace KY.Generator.Commands
{
    internal class StatisticsCommand : GeneratorCommand<StatisticsCommandParameters>
    {
        private readonly GlobalStatisticsService globalStatisticsService;
        private readonly StatisticsService statisticsService;
        public override string[] Names { get; } = { "statistics", "statistic", "stats", "stat" };

        public StatisticsCommand(GlobalStatisticsService globalStatisticsService, StatisticsService statisticsService)
        {
            this.globalStatisticsService = globalStatisticsService;
            this.statisticsService = statisticsService;
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
            this.statisticsService.Anonymize(statistic);
            this.statisticsService.Submit(statistic);
            return this.Success();
        }
    }
}
