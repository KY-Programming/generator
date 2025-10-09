using KY.Generator.Extensions;
using KY.Generator.Output;
using KY.Generator.Statistics;

namespace KY.Generator.Command;

public class GeneratorCommandRunner(IOutput output, StatisticsService statisticsService)
{
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
        if (!string.IsNullOrEmpty(command.Parameters.Output))
        {
            output.Move(command.Parameters.Output);
        }
        if (!command.Parameters.SkipAsyncCheck)
        {
            if (!command.Parameters.IsOnlyAsync && command.Parameters.IsAsync)
            {
                return new SwitchAsyncResult();
            }
            bool? isAssemblyAsync = command.Parameters.IsAsyncAssembly;
            if (!command.Parameters.IsAsync)
            {
                if (!string.IsNullOrEmpty(command.Parameters.Assembly))
                {
                    LocateAssemblyResult locateAssemblyResult = GeneratorAssemblyLocator.Locate(command.Parameters.Assembly, command.Parameters.IsBeforeBuild);
                    if (locateAssemblyResult.SwitchContext)
                    {
                        return locateAssemblyResult;
                    }
                    isAssemblyAsync = locateAssemblyResult.Assembly?.IsAsync();
                }
            }
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
        Measurement measurement = statisticsService.StartMeasurement();
        try
        {
            return command.Run();
        }
        finally
        {
            statisticsService.Measure(measurement, command);
        }
    }
}
