using KY.Core.Dependency;
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

    public List<IGeneratorCommand> Create(IEnumerable<GeneratorCommandParameters> parameters, IDependencyResolver? resolver = null)
    {
        return this.commandFactory.Create(parameters, resolver);
    }

    public async Task<IGeneratorCommandResult> Run(IGeneratorCommand command)
    {
        if (!command.Parameters.SkipBackgroundCheck)
        {
            if (!command.Parameters.IsBackgroundRun && command.Parameters.IsInBackground)
            {
                return new SwitchToBackgroundResult();
            }
            bool? isAssemblyInBackground = command.Parameters.IsBackgroundAssembly;
            if (isAssemblyInBackground != null)
            {
                if (!command.Parameters.IsBackgroundRun && isAssemblyInBackground.Value)
                {
                    return new SwitchToBackgroundResult();
                }
                if (command.Parameters.IsBackgroundRun && !command.Parameters.IsInBackground && !isAssemblyInBackground.Value)
                {
                    return new SwitchToBackgroundResult();
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
