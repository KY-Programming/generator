using System.Collections.Generic;
using System.Linq;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Output;
using KY.Generator.TypeScript.Transfer;
using KY.Generator.TypeScript.Transfer.Readers;

namespace KY.Generator.TypeScript;

public static class TypeScriptStrictHelper
{
    private static readonly Dictionary<string, TsConfig> cache = new();

    /// <summary>
    /// Reads the strict mode from the tsconfig.json of the output folder into
    /// <see cref="TypeScriptOptions.StrictFromConfig"/>. It is only a fallback for the strict-by-default behaviour
    /// and never overrules an explicitly set strict mode, so it can always be read.
    /// </summary>
    public static void SetStrictFromConfig(this TypeScriptOptions options, string? relativePath, IDependencyResolver resolver)
    {
        if (relativePath == null)
        {
            return;
        }
        options.StrictFromConfig = Read(relativePath, resolver);
    }

    /// <summary>
    /// Returns the strict mode configured in the tsconfig.json of the given output folder, or <c>null</c> if no
    /// tsconfig.json could be found or it does not configure a strict mode. An absent entry is deliberately not
    /// read as false: since TypeScript 6 the compiler defaults it to true.
    /// </summary>
    public static bool? Read(string relativePath, IDependencyResolver resolver)
    {
        if (resolver.Get<IOutput>() is FileOutput fileOutput)
        {
            string fullPath = FileSystem.Combine(fileOutput.BasePath, relativePath);
            TsConfig tsConfig = cache.FirstOrDefault(x => fullPath.StartsWith(x.Key)).Value;
            if (tsConfig == null)
            {
                tsConfig = resolver.Create<TsConfigReader>().Read(fullPath);
                if (tsConfig?.Path != null)
                {
                    string basePath = FileSystem.GetDirectoryName(tsConfig.Path);
                    cache[basePath] = tsConfig;
                }
            }
            return tsConfig?.CompilerOptions?.Strict;
        }
        return null;
    }
}
