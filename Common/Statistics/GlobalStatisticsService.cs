using System;
using System.Collections.Generic;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Commands;
using KY.Generator.Extensions;
using KY.Generator.Models;
using Newtonsoft.Json;

namespace KY.Generator.Statistics
{
    public class GlobalStatisticsService
    {
        private readonly IDependencyResolver resolver;
        private GlobalStatistic cache;
        private readonly string fileName;

        public GlobalStatisticsService(IDependencyResolver resolver, IEnvironment environment)
        {
            this.resolver = resolver;
            this.fileName = FileSystem.Combine(environment.ApplicationData, "global.statistics.json");
        }

        public GlobalStatistic Read()
        {
            if (this.cache == null)
            {
                if (FileSystem.FileExists(this.fileName))
                {
                    Logger.Trace("Read global statistics...");
                    this.cache = JsonConvert.DeserializeObject<GlobalStatistic>(FileSystem.ReadAllText(this.fileName));
                }
                this.cache ??= new GlobalStatistic();
            }
            return this.cache;
        }

        public void Append(Statistic statistic)
        {
            GlobalStatistic data = this.Read();
            data.AssertIsNotNull(null, "No data found. Use Read() method first");
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

        public void Write()
        {
            this.cache.AssertIsNotNull(null, "No data found. Use Read() method first");
            FileSystem.WriteAllText(this.fileName, JsonConvert.SerializeObject(this.cache));
        }

        public void StartCalculation(string statisticsFileName)
        {
            StatisticsCommand command = this.resolver.Create<StatisticsCommand>();
            command.Parameters.File = statisticsFileName;
            command.Parameters.Force = true;
            GeneratorProcess.StartHidden(command);
        }

        public List<Guid> GetIds()
        {
            return this.Read()?.Ids ?? new List<Guid>();
        }
    }
}
