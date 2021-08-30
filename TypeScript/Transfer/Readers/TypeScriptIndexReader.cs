using System.Linq;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;

namespace KY.Generator.TypeScript.Transfer.Readers
{
    public class TypeScriptIndexReader
    {
        private readonly IEnvironment environment;
        private static readonly Regex regex = new(@"\s*(export\s*(?<types>(\*|{[^}]+}))\s*from\s*['""](?<path>[^'""]+)['""];?|(?<fallback>.*))");

        public TypeScriptIndexReader(IEnvironment environment)
        {
            this.environment = environment;
        }

        public TypeScriptIndexFile Read(string relativePath)
        {
            string fullPath = FileSystem.Combine(this.environment.OutputPath, relativePath, "index.ts");
            if (!FileSystem.FileExists(fullPath))
            {
                return null;
            }
            string fileContent = FileSystem.ReadAllText(fullPath);
            return this.Parse(fileContent);
        }

        public TypeScriptIndexFile Parse(string value)
        {
            Match match = regex.Match(value);
            if (!match.Success)
            {
                Logger.Error($"Could not parse index.ts. Please contact support@ky-programming.de");
                return null;
            }

            TypeScriptIndexFile file = new();
            Match currentMatch = match;
            do
            {
                string types = currentMatch.Groups["types"].Value.Trim();
                string path = currentMatch.Groups["path"].Value.Trim();
                string fallback = currentMatch.Groups["fallback"].Value.Trim();

                if (!string.IsNullOrEmpty(types) && !string.IsNullOrEmpty(path))
                {
                    file.Lines.Add(new ExportIndexLine
                                   {
                                       Types = types.Trim(' ', '{', '}').Split(',').Select(x => x.Trim()).ToList(),
                                       Path = path
                                   }
                    );
                }
                else if (!string.IsNullOrEmpty(fallback))
                {
                    file.Lines.Add(new UnknownIndexLine { Content = fallback });
                }
                currentMatch = currentMatch.NextMatch();
            } while (currentMatch.Success);
            return file;
        }
    }
}
