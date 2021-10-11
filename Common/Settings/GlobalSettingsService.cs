using System;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;
using KY.Generator.Statistics;
using Newtonsoft.Json;

namespace KY.Generator.Settings
{
    public class GlobalSettingsService
    {
        private GlobalSettings cache;
        private readonly string fileName;

        public GlobalSettingsService(IEnvironment environment)
        {
            this.fileName = FileSystem.Combine(environment.ApplicationData, "global.settings.json");
        }

        public GlobalSettings Read()
        {
            if (this.cache == null)
            {
                if (FileSystem.FileExists(this.fileName))
                {
                    Logger.Trace("Read global settings...");
                    this.cache = JsonConvert.DeserializeObject<GlobalSettings>(FileSystem.ReadAllText(this.fileName));
                }
                this.cache ??= new GlobalSettings();
                this.Write();
            }
            return this.cache;
        }

        public void Write()
        {
            this.cache.AssertIsNotNull(null, "No data found. Use Read() method first");
            FileSystem.WriteAllText(this.fileName, JsonConvert.SerializeObject(this.cache));
        }

        public void ReadAndWrite(Action<GlobalSettings> action)
        {
            action(this.Read());
            this.Write();
        }
    }
}
