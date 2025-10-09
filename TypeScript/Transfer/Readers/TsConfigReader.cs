using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Transfer;
using Newtonsoft.Json;

namespace KY.Generator.TypeScript.Transfer.Readers
{
    public class TsConfigReader
    {
        private readonly List<ITransferObject> transferObjects;
        private static readonly Dictionary<string, TsConfig> cache = new();
        private static readonly Regex pathRegex = new(@"(?<path>.*ClientApp[^\\\/]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public TsConfigReader(List<ITransferObject> transferObjects)
        {
            this.transferObjects = transferObjects;
        }

        public TsConfig Read(string fullPath)
        {
            TsConfig tsConfig = transferObjects.OfType<TsConfig>().FirstOrDefault();
            if (tsConfig != null)
            {
                return tsConfig;
            }
            if (cache.TryGetValue(fullPath, out TsConfig read))
            {
                this.LogInfo(read);
                return read;
            }
            string path = FileSystem.Combine(fullPath, "tsconfig.json");
            Logger.Trace($"Try to read strict mode from {path}");
            if (!FileSystem.FileExists(path))
            {
                Match match = pathRegex.Match(path);
                if (match.Success)
                {
                    string basePath = match.Groups["path"].Value;
                    path = FileSystem.Combine(basePath, "tsconfig.json");
                    Logger.Trace($"Try to read strict mode from {path}");
                }
            }
            if (!FileSystem.FileExists(path) && fullPath.Contains("src"))
            {
                path = FileSystem.Combine(fullPath.Substring(0, fullPath.IndexOf("src")), "tsconfig.json");
                Logger.Trace($"Try to read strict mode from {path}");
            }
            TsConfig config;
            if (FileSystem.FileExists(path))
            {
                config = this.Parse(path);
                this.LogInfo(config);
            }
            else
            {
                Logger.Trace("Could not find tsconfig.json");
                config = null;
            }
            cache[fullPath] = config;
            return config;
        }

        private TsConfig Parse(string path)
        {
            string text = FileSystem.ReadAllText(path);
            TsConfig tsConfig = JsonConvert.DeserializeObject<TsConfig>(text);
            tsConfig.Path = path;
            this.transferObjects.Add(tsConfig);
            return tsConfig;
        }

        private void LogInfo(TsConfig config)
        {
            Logger.Trace($"Activate TypeScript {(config?.CompilerOptions?.Strict == true ? "strict" : "regular")} mode");
        }
    }
}
