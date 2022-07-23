using System.Collections.Generic;
using System.Linq;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Output;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Transfer;
using KY.Generator.TypeScript.Transfer.Readers;

namespace KY.Generator.TypeScript
{
    public static class TypeScriptStrictHelper
    {
        private static readonly Dictionary<string, TsConfig> cache = new();

        public static void SetStrict(this IOptions options, string relativePath, IDependencyResolver resolver)
        {
            if (options.IsStrictSet)
            {
                return;
            }
            options.Strict = Read(relativePath, resolver);
        }

        public static bool Read(string relativePath, IDependencyResolver resolver)
        {
            if (resolver.Get<IOutput>() is FileOutput fileOutput)
            {
                string fullPath = FileSystem.Combine(fileOutput.BasePath, relativePath);
                TsConfig tsConfig = cache.FirstOrDefault(x => fullPath.StartsWith(x.Key)).Value;
                if (tsConfig == null)
                {
                    tsConfig = resolver.Create<TsConfigReader>().Read(fullPath);
                    if (tsConfig != null)
                    {
                        string basePath = FileSystem.GetDirectoryName(tsConfig.Path);
                        cache[basePath] = tsConfig;
                    }
                }
                return tsConfig?.CompilerOptions?.Strict ?? false;
            }
            return false;
        }
    }
}
