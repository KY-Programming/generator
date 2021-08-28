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
            files.ForEach(file => file.Write(output));
        }

        public static void Write(this FileTemplate file, IOutput output)
        {
            FileWriter writer = new(file.Options);
            writer.Add(file);
            output.Write(file.FullPath, writer.ToString());
        }
    }
}
