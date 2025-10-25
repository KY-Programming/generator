using KY.Core;
using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class OptionsCommand : GeneratorCommand<OptionsCommandParameters>, IPrepareCommand
{
    private readonly StatisticsService statisticsService;
    private readonly GlobalStatisticsService globalStatisticsService;
    private readonly IOutput output;
    private readonly IEnvironment environment;

    public OptionsCommand(StatisticsService statisticsService, GlobalStatisticsService globalStatisticsService, IOutput output, IEnvironment environment)
    {
        this.statisticsService = statisticsService;
        this.globalStatisticsService = globalStatisticsService;
        this.output = output;
        this.environment = environment;
    }

    public override IGeneratorCommandResult Run()
    {
        if (this.Parameters.Statistics != null)
        {
            List<Guid> ids = this.globalStatisticsService.GetIds();
            switch (this.Parameters.Statistics?.ToLowerInvariant())
            {
                case "false":
                case "off":
                case "disable":
                    this.statisticsService.Disable(ids);
                    break;
                case "true":
                case "on":
                case "enable":
                    this.statisticsService.Enable(ids);
                    break;
                default:
                    Logger.Error($"Invalid value '{this.Parameters.Statistics}' for option 'statistics'. Valid values are 'enable' or 'disable'");
                    return this.Error();
            }
        }
        else if (this.Parameters.Output != null)
        {
            this.environment.OutputPath = this.Parameters.Output;
            this.output.Move(this.Parameters.Output);
        }
        else
        {
            Logger.Error($"Unknown or missing option found.");
            return this.Error();
        }
        return this.Success();
    }
}
