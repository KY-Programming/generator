using System.Collections.Generic;
using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static class FileTemplateExtension
    {
        public static void Write(this IEnumerable<FileTemplate> files, IOutput output)
        {
#if DEBUG
            files.ForEach(file => file.Write(output));
#else
            System.Threading.Tasks.Parallel.ForEach(files, file => file.Write(output));
#endif
        }

        public static void Write(this FileTemplate file, IOutput output)
        {
            // Logger.Trace($"Start generate file {file.Name}");
            FileWriter writer = new(file.Options);
            writer.Add(file);
            output.Write(file.FullPath, writer.ToString(), !file.WriteOutputId, file.ForceOverwrite);
            // Logger.Trace($"Finish generate file {file.Name}");
        }
    }
}
