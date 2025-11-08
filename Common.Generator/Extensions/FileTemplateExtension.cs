using System.Collections.Generic;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator;

public static class FileTemplateExtension
{
    public static void Write(this IEnumerable<FileTemplate> files, IOutput output, IDependencyResolver resolver)
    {
        files.ForEach(file => file.Write(output, resolver));
    }

    public static void Write(this FileTemplate file, IOutput output, IDependencyResolver resolver)
    {
        // Logger.Trace($"Start generate file {file.Name}");
        FileWriter writer = resolver.Create<FileWriter>(file.Options);
        writer.Add(file);
        output.Write(file.FullPath, writer.ToString(), file.Options, !file.WriteOutputId, file.ForceOverwrite);
        // Logger.Trace($"Finish generate file {file.Name}");
    }
}
