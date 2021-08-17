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
        private static readonly Regex pathRegex = new(@"(?<path>.*ClientApp[^\\\/]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public TsConfig Read(string fullPath, List<ITransferObject> transferObjects)
        {
            TsConfig tsConfig = transferObjects.OfType<TsConfig>().FirstOrDefault();
            if (tsConfig != null)
            {
                return tsConfig;
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
                }
                Logger.Trace($"Try to read strict mode from {path}");
            }
            if (!FileSystem.FileExists(path) && fullPath.Contains("src"))
            {
                path = FileSystem.Combine(fullPath.Substring(0, fullPath.IndexOf("src")), "tsconfig.json");
                Logger.Trace($"Try to read strict mode from {path}");
            }
            if (FileSystem.FileExists(path))
            {
                return this.Parse(path, transferObjects);
            }
            Logger.Trace("Could not find tsconfig.json");
            return null;
        }

        private TsConfig Parse(string path, List<ITransferObject> transferObjects)
        {
            string text = FileSystem.ReadAllText(path);
            TsConfig tsConfig = JsonConvert.DeserializeObject<TsConfig>(text);
            transferObjects.Add(tsConfig);
            Logger.Trace($"Activate TypeScript {(tsConfig?.CompilerOptions?.Strict == true ? "strict" : "regular")} mode");
            return tsConfig;
        }
    }
}
