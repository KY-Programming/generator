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
        private static readonly Regex pathRegex = new Regex(@"(?<path>.*ClientApp[^\\\/]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void Read(string fullPath, List<ITransferObject> transferObjects)
        {
            if (transferObjects.OfType<TsConfig>().Any())
            {
                return;
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
                this.Parse(path, transferObjects);
            }
            else
            {
                Logger.Trace("Could not find tsconfig.json");
            }
        }

        private void Parse(string path, List<ITransferObject> transferObjects)
        {
            string text = FileSystem.ReadAllText(path);
            TsConfig tsConfig = JsonConvert.DeserializeObject<TsConfig>(text);
            transferObjects.Add(tsConfig);
            Logger.Trace($"Activate TypeScript {(tsConfig?.CompilerOptions?.Strict == true ? "strict" : "regular")} mode");
        }
    }
}
