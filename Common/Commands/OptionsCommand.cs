using KY.Core;
using KY.Generator.Command;
using KY.Generator.Settings;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class OptionsCommand : GeneratorCommand<OptionsCommandParameters>
{
    private readonly StatisticsService statisticsService;
    private readonly GlobalStatisticsService globalStatisticsService;
    private readonly GlobalSettingsService globalSettingsService;
    public static string[] Names { get; } = [ToCommand(nameof(OptionsCommand)), "set"];

    public OptionsCommand(StatisticsService statisticsService, GlobalStatisticsService globalStatisticsService, GlobalSettingsService globalSettingsService)
    {
        this.statisticsService = statisticsService;
        this.globalStatisticsService = globalStatisticsService;
        this.globalSettingsService = globalSettingsService;
    }

    public override IGeneratorCommandResult Run()
    {
        if ("statistics".Equals(this.Parameters.Option, StringComparison.CurrentCultureIgnoreCase))
        {
            List<Guid> ids = this.globalStatisticsService.GetIds();
            if ("disable".Equals(this.Parameters.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                this.statisticsService.Disable(ids);
            }
            else if ("enable".Equals(this.Parameters.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                this.statisticsService.Enable(ids);
            }
            else
            {
                Logger.Error($"Invalid value '{this.Parameters.Value}' for option '{this.Parameters.Option}'. Valid values are 'enable' or 'disable'");
                return this.Error();
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
