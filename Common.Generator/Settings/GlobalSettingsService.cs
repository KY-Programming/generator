using System.Diagnostics;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Extension;
using KY.Generator.Models;
using Newtonsoft.Json;

namespace KY.Generator.Settings;

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
            Stopwatch stopwatch = new();
            stopwatch.Start();
            if (FileSystem.FileExists(this.fileName))
            {
                try
                {
                    this.cache = JsonConvert.DeserializeObject<GlobalSettings>(FileSystem.ReadAllText(this.fileName));
                }
                catch (Exception exception)
                {
                    Logger.Warning("Could not read global settings." + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace);
                    this.cache = null;
                }
            }
            if (this.cache == null)
            {
                this.cache = new GlobalSettings();
                this.Write();
            }
            stopwatch.Stop();
            Logger.Trace($"Global settings read in {stopwatch.FormattedElapsed()}");
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
