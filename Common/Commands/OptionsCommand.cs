using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Statistics;

namespace KY.Generator.Commands
{
    internal class OptionsCommand : GeneratorCommand<OptionsCommandParameters>
    {
        private readonly GlobalOptions globalOptions;
        private readonly StatisticsService statisticsService;
        private readonly GlobalStatisticsService globalStatisticsService;
        public override string[] Names { get; } = { "set" };

        public OptionsCommand(GlobalOptions globalOptions, StatisticsService statisticsService, GlobalStatisticsService globalStatisticsService)
        {
            this.globalOptions = globalOptions;
            this.statisticsService = statisticsService;
            this.globalStatisticsService = globalStatisticsService;
        }

        public override IGeneratorCommandResult Run()
        {
            if ("statistics".Equals(this.Parameters.Option, StringComparison.CurrentCultureIgnoreCase))
            {
                if ("disable".Equals(this.Parameters.Value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.globalOptions.StatisticsEnabled = false;
                }
                else if ("enable".Equals(this.Parameters.Value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.globalOptions.StatisticsEnabled = true;
                }
                else
                {
                    Logger.Error($"Invalid value '{this.Parameters.Value}' for option '{this.Parameters.Option}'. Valid values are 'enable' or 'disable'");
                    return this.Error();
                }
                this.globalOptions.Save();
                List<Guid> ids = this.globalStatisticsService.GetIds();
                if (ids.Count > 0)
                {
                    if (this.globalOptions.StatisticsEnabled)
                    {
                        this.statisticsService.Enable(ids);
                    }
                    else
                    {
                        this.statisticsService.Disable(ids);
                    }
                }
            }
            else
            {
                Logger.Error($"Unknown option \"{this.Parameters.Option}\" found.");
                return this.Error();
            }
            return this.Success();
        }
    }
}
