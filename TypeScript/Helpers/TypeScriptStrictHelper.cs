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
        public static void Read(string relativePath, IOutput output, IDependencyResolver resolver, List<ITransferObject> transferObjects)
        {
            if (output is FileOutput fileOutput)
            {
                resolver.Create<TsConfigReader>().Read(FileSystem.Combine(fileOutput.BasePath, relativePath), transferObjects);
            }
        }
    }
}
