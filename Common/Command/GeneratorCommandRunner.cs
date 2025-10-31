using KY.Generator.Statistics;

namespace KY.Generator.Command;

public class GeneratorCommandRunner
{
    private readonly StatisticsService statisticsService;
    private readonly GeneratorCommandFactory commandFactory;

    public GeneratorCommandRunner(StatisticsService statisticsService, GeneratorCommandFactory commandFactory)
    {
        this.statisticsService = statisticsService;
        this.commandFactory = commandFactory;
    }

    public async Task<IGeneratorCommandResult> Run(IEnumerable<IGeneratorCommand> commands)
    {
        List<IGeneratorCommand> list = commands.ToList();
        IGeneratorCommandResult? result = null;
        list.ForEach(command => command.Prepare());
        foreach (IGeneratorCommand command in list)
        {
            result = await this.Run(command);
            if (!result.Success)
            {
                return result;
            }
        }
        return result ?? new SuccessResult();
    }

    public async Task<IGeneratorCommandResult> Run(IEnumerable<GeneratorCommandParameters> parameters)
    {
        List<IGeneratorCommand> commands = this.commandFactory.Create(parameters);
        IGeneratorCommandResult? result = null;
        commands.ForEach(command => command.Prepare());
        foreach (IGeneratorCommand command in commands)
        {
            result = await this.Run(command);
            if (!result.Success)
            {
                return result;
            }
        }
        foreach (IGeneratorCommand command in commands)
        {
            command.FollowUp();
        }
        return result ?? new SuccessResult();
    }

    public async Task<IGeneratorCommandResult> Run(IGeneratorCommand command)
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
            return await command.Run();
        }
        finally
        {
            this.statisticsService.Measure(measurement, command);
        }
    }
}
