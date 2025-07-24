using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using KY.Core;
using KY.Core.Crypt;
using KY.Core.DataAccess;
using KY.Generator.Models;
using Newtonsoft.Json;

namespace KY.Generator;

public class AssemblyCache : IAssemblyCache
{
    private readonly string globalCacheFileName = "global-assembly-cache.json";
    private string localCacheFileName;
    private readonly IEnvironment environment;
    private bool hasGlobalChanges;
    private Dictionary<string, string> global = new();
    private bool hasLocalChanges;
    private Dictionary<string, string> local = new();
    private readonly List<string> globalPaths;

    public AssemblyCache(IEnvironment environment)
    {
        this.environment = environment;
        this.LoadGlobal();
        this.globalPaths = InstalledRuntime.GetCurrent().Select(x => x.Path).ToList();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            this.globalPaths.Add(NugetPackageDependencyLoader.WindowsNugetFallbackPath);
            this.globalPaths.Add(NugetPackageDependencyLoader.WindowsNugetCachePath);
        }
        else
        {
            this.globalPaths.Add(NugetPackageDependencyLoader.LinuxNugetCachePath);
        }
    }

    public void Add(string name, string location)
    {
        if (name.StartsWith("KY.Generator"))
        {
            return;
        }
        if (this.globalPaths.Any(location.StartsWith))
        {
            this.hasGlobalChanges = true;
            this.global[name] = location;
        }
        else
        {
            this.hasLocalChanges = true;
            this.local[name] = location;
        }
    }

    public void LoadGlobal()
    {
        string fileName = FileSystem.Combine(this.environment.LocalApplicationData, this.globalCacheFileName);
        if (FileSystem.FileExists(fileName))
        {
            string json = FileSystem.ReadAllText(fileName);
            this.global = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            this.hasGlobalChanges = false;
        }
    }

    public void LoadLocal()
    {
        if (this.environment.ProjectFile == null)
        {
            return;
        }
        string hash = Sha1.Create(this.environment.ProjectFile).ToString().Substring(0, 8);
        this.localCacheFileName = $"{this.environment.Name}-{hash}-assembly-cache.json";
        string fileName = FileSystem.Combine(this.environment.LocalApplicationData, this.localCacheFileName);
        if (FileSystem.FileExists(fileName))
        {
            string json = FileSystem.ReadAllText(fileName);
            this.local = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            this.hasLocalChanges = false;
        }
    }

    public string Resolve(string name)
    {
        if (this.global.TryGetValue(name, out string globalLocation))
        {
            Logger.Trace($"Assembly found in global assembly cache: {globalLocation}");
            return globalLocation;
        }
        if (this.local.TryGetValue(name, out string localLocation))
        {
            Logger.Trace($"Assembly found in local assembly cache: {localLocation}");
            return localLocation;
        }
        return null;
    }

    public void Save()
    {
        if (this.global.Count > 0 && this.hasGlobalChanges)
        {
            string fileName = FileSystem.Combine(this.environment.LocalApplicationData, this.globalCacheFileName);
            FileSystem.WriteAllText(fileName, JsonConvert.SerializeObject(this.global));
        }
        if (this.local.Count > 0 && this.hasLocalChanges && this.localCacheFileName != null)
        {
            string fileName = FileSystem.Combine(this.environment.LocalApplicationData, this.localCacheFileName);
            FileSystem.WriteAllText(fileName, JsonConvert.SerializeObject(this.local));
        }
    }
}
