using System.Collections.Generic;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Output;
using KY.Generator.Transfer;
using KY.Generator.TypeScript.Transfer.Readers;

namespace KY.Generator.TypeScript
{
    public static class TypeScriptStrictHelper
    {
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
                return resolver.Create<TsConfigReader>().Read(FileSystem.Combine(fileOutput.BasePath, relativePath))?.CompilerOptions?.Strict ?? false;
            }
            return false;
        }
    }
}
