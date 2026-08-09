using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Statistics;

namespace KY.Generator.Commands;

internal class CleanupCommand : GeneratorCommand<CleanupCommandParameters>
{
    private readonly StatisticsService statisticsService;

    public CleanupCommand(StatisticsService statisticsService)
    {
        this.statisticsService = statisticsService;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        if (this.Parameters.Logs)
        {
            this.CleanupLogs();
        }
        if (this.Parameters.Statistics)
        {
            int deleted = this.statisticsService.Cleanup();
            Logger.Trace($"{deleted} statistic {deleted} files deleted");
        }
        return this.SuccessAsync();
    }

    private void CleanupLogs()
    {
        string path = Logger.File.Path;
        if (string.IsNullOrEmpty(path) || !FileSystem.DirectoryExists(path))
        {
            Logger.Trace($"0 log files deleted. Directory \"{path}\" not found");
            return;
        }
        int deleted = 0;
        int skipped = 0;
        foreach (string file in FileSystem.GetFiles(path, "*.log"))
        {
            try
            {
                FileSystem.DeleteFile(file);
                deleted++;
            }
            catch (Exception exception)
            {
                skipped++;
                Logger.Warning($"Can not delete log file \"{file}\": {exception.Message}");
            }
        }
        string skippedInfo = skipped > 0 ? $" ({skipped} files skipped)" : string.Empty;
        Logger.Trace($"{deleted} log files deleted {skippedInfo}.");
    }
}
