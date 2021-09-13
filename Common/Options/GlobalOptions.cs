using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;
using Newtonsoft.Json;

namespace KY.Generator
{
    public interface IGlobalOptions
    {
        public bool StatisticsEnabled { get; set; }
    }

    public class GlobalOptions : IGlobalOptions
    {
        private readonly IEnvironment environment;
        private readonly string filePath;

        public bool StatisticsEnabled { get; set; } = true;

        public GlobalOptions(IEnvironment environment)
        {
            this.environment = environment;
            this.filePath = FileSystem.Combine(this.environment.ApplicationData, "options.json");
            this.Load();
        }

        public void Load()
        {
            if (!FileSystem.FileExists(this.filePath))
            {
                return;
            }
            JsonConvert.PopulateObject(FileSystem.ReadAllText(this.filePath), this);
        }

        public void Save()
        {
            FileSystem.WriteAllText(this.filePath, JsonConvert.SerializeObject(this));
        }
    }
}
