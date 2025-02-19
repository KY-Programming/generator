﻿using KY.Generator.Command;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

public class CleanupCommand : GeneratorCommand<CleanupCommandParameters>
{
    private readonly StatisticsService statisticsService;
    public static string[] Names { get; } = [ToCommand(nameof(CleanupCommand)), "cleanup"];

    public CleanupCommand(StatisticsService statisticsService)
    {
        this.statisticsService = statisticsService;
    }

    public override IGeneratorCommandResult Run()
    {
        if (this.Parameters.Logs)
        {
            // TODO: Cleanup logs
        }
        if (this.Parameters.Statistics)
        {
            this.statisticsService.Cleanup();
        }
        return this.Success();
    }
}
