using KY.Generator.Statistics;

namespace KY.Generator.Command;

public class GeneratorCommandRunner
{
    private readonly StatisticsService statisticsService;

    public GeneratorCommandRunner(StatisticsService statisticsService)
    {
        this.statisticsService = statisticsService;
    }

    public IGeneratorCommandResult Run(IEnumerable<IGeneratorCommand> commands)
    {
        List<IGeneratorCommand> list = commands.ToList();
        IGeneratorCommandResult? result = null;
        list.ForEach(command => command.Prepare());
        foreach (IGeneratorCommand command in list)
        {
            result = this.Run(command);
            if (!result.Success)
            {
                return result;
            }
        }
        return result ?? new SuccessResult();
    }

    public IGeneratorCommandResult Run(IGeneratorCommand command)
    {
        if (!command.Parameters.SkipAsyncCheck)
        {
            if (!command.Parameters.IsOnlyAsync && command.Parameters.IsAsync)
            {
                return new SwitchAsyncResult();
            }
            bool? isAssemblyAsync = command.Parameters.IsAsyncAssembly;
            if (isAssemblyAsync != null)
            {
                if (!command.Parameters.IsOnlyAsync && isAssemblyAsync.Value)
                {
                    return new SwitchAsyncResult();
                }
                if (command.Parameters.IsOnlyAsync && !command.Parameters.IsAsync && !isAssemblyAsync.Value)
                {
                    return new SwitchAsyncResult();
                }
            }
        }
        Measurement measurement = this.statisticsService.StartMeasurement();
        try
        {
            return command.Run();
        }
        finally
        {
            this.statisticsService.Measure(measurement, command);
        }
    }
}
