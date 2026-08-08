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

        /// <summary>
        /// Every tsconfig.json path that was probed once, with the config that was found there or null if there
        /// was none. Different output folders share their candidates - e.g. the models and the services folder of
        /// the same ClientApp - so a path that does not exist must not be probed again for the next folder.
        /// </summary>
        private static readonly Dictionary<string, TsConfig> probed = new();
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
            TsConfig config = this.Find(fullPath);
            cache[fullPath] = config;
            return config;
        }

        private TsConfig Find(string fullPath)
        {
            foreach (string path in CandidatePaths(fullPath))
            {
                if (probed.TryGetValue(path, out TsConfig known))
                {
                    if (known == null)
                    {
                        continue;
                    }
                    this.LogInfo(known);
                    return known;
                }
                Logger.Trace($"Try to read strict mode from {path}");
                if (!FileSystem.FileExists(path))
                {
                    probed[path] = null;
                    continue;
                }
                TsConfig config = this.Parse(path);
                probed[path] = config;
                this.LogInfo(config);
                return config;
            }
            Logger.Trace("Could not find tsconfig.json");
            return null;
        }

        /// <summary>
        /// The tsconfig.json locations that belong to an output folder, in the order they are tried: next to the
        /// output itself, next to the ClientApp it lives in and next to its src folder.
        /// </summary>
        private static IEnumerable<string> CandidatePaths(string fullPath)
        {
            yield return FileSystem.Combine(fullPath, "tsconfig.json");

            Match match = pathRegex.Match(fullPath);
            if (match.Success)
            {
                yield return FileSystem.Combine(match.Groups["path"].Value, "tsconfig.json");
            }
            int index = fullPath.IndexOf("src");
            if (index >= 0)
            {
                yield return FileSystem.Combine(fullPath.Substring(0, index), "tsconfig.json");
            }
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
            switch (config?.CompilerOptions?.Strict)
            {
                case true:
                    Logger.Trace("Activate TypeScript strict mode");
                    break;
                case false:
                    Logger.Trace("Activate TypeScript regular mode");
                    break;
                default:
                    Logger.Trace("No strict mode configured, keep the default");
                    break;
            }
        }
    }
}
