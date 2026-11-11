using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Commands;
using KY.Generator.Models;
using Newtonsoft.Json;

namespace KY.Generator.Statistics;

public class GlobalStatisticsService
{
    private readonly IDependencyResolver resolver;
    private GlobalStatistic? cache;
    private readonly string fileName;

    public GlobalStatisticsService(IDependencyResolver resolver, IEnvironment environment)
    {
        this.resolver = resolver;
        this.fileName = FileSystem.Combine(environment.ApplicationData, "global.statistics.json");
    }

    public GlobalStatistic Read()
    {
        return this.cache ??= this.ReadFile();
    }

    public void Update(Statistic statistic)
    {
        using IDisposable fileLock = FileLock.Acquire(this.fileName);
        GlobalStatistic data = this.cache = this.ReadFile();
        Append(data, statistic);
        FileSystem.WriteAllText(this.fileName, JsonConvert.SerializeObject(data));
    }

    private GlobalStatistic ReadFile()
    {
        if (!FileSystem.FileExists(this.fileName))
        {
            return new GlobalStatistic();
        }
        Logger.Trace("Read global statistics...");
        try
        {
            return JsonConvert.DeserializeObject<GlobalStatistic>(FileSystem.ReadAllText(this.fileName)) ?? new GlobalStatistic();
        }
        catch (Exception exception)
        {
            Logger.Warning($"Could not read {this.fileName}. The global statistics start over." + Environment.NewLine + exception.Message);
            return new GlobalStatistic();
        }
    }

    private static void Append(GlobalStatistic data, Statistic statistic)
    {
        if (data.Today != DateTime.Today)
        {
            data.TodayFiles = 0;
            data.TodayLines = 0;
        }
        data.Today = DateTime.Today;
        data.Files += statistic.GeneratedFiles;
        data.Lines += statistic.OutputLines;
        data.TodayFiles += statistic.GeneratedFiles;
        data.TodayLines += statistic.OutputLines;
        data.Ids.AddIfNotExists(statistic.Id);
    }

    public void StartCalculation(string statisticsFileName)
    {
        ForceCommand forceCommand = this.resolver.Create<ForceCommand>();
        StatisticsCommand statisticsCommand = this.resolver.Create<StatisticsCommand>();
        statisticsCommand.Parameters.File = statisticsFileName;
        GeneratorProcess.StartHidden(forceCommand, statisticsCommand);
    }

    public List<Guid> GetIds()
    {
        return this.Read()?.Ids ?? new List<Guid>();
    }
}
