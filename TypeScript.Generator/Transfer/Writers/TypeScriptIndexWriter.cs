using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Transfer.Writers;

public class TypeScriptIndexWriter : TransferWriter
{
    private readonly List<ITransferObject> transferObjects;
    private readonly List<FileTemplate> files;

    public TypeScriptIndexWriter(Options options, ITypeMapping typeMapping, List<ITransferObject> transferObjects, List<FileTemplate> files)
        : base(options, typeMapping)
    {
        this.transferObjects = transferObjects;
        this.files = files;
    }

    public virtual void Write()
    {
        Logger.Trace("Generate index.ts...");
        foreach (TypeScriptIndexFile file in this.transferObjects.OfType<TypeScriptIndexFile>())
        {
            this.Write(file, file.RelativePath);
        }
    }

    public virtual void Write(TypeScriptIndexFile file, string relativePath)
    {
        if (file == null)
        {
            return;
        }
        GeneratorOptions generatorOptions = this.Options.Get<GeneratorOptions>();
        string fileName = Formatter.FormatFile("index", generatorOptions);
        FileTemplate fileTemplate = this.files.FirstOrDefault(file => file.Name == fileName && Compare(file.RelativePath, relativePath))
                                    ?? this.files.AddFile(relativePath, generatorOptions);
        fileTemplate.WithName(fileName)
                    .ForceOverwrite()
                    .NoHeader()
                    .NoLint();
        fileTemplate.Usings.Clear();
        foreach (IIndexLine line in file.Lines)
        {
            if (line is ExportIndexLine indexLine)
            {
                foreach (string type in indexLine.Types)
                {
                    fileTemplate.AddExport(type, indexLine.Path.TrimEnd(".ts"));
                }
            }
            else if (line is UnknownIndexLine unknownIndexLine)
            {
                fileTemplate.Usings.Add(new UnknownExportTemplate(Code.TypeScript(unknownIndexLine.Content)));
            }
        }
        foreach (ExportTemplate exportTemplate in fileTemplate.Usings.OfType<ExportTemplate>().ToList())
        {
            if (fileTemplate.Usings.Count(u => u.Type == exportTemplate.Type && u.Path.Equals(exportTemplate.Path, StringComparison.InvariantCultureIgnoreCase)) > 1)
            {
                fileTemplate.Usings.Remove(exportTemplate);
            }
        }
    }

    private static bool Compare(string relativePath, string otherRelativePath)
    {
        return Normalize(relativePath) == Normalize(otherRelativePath);
    }

    private static string Normalize(string relativePath)
    {
        return (relativePath ?? string.Empty).Replace("\\", "/").TrimEnd('/');
    }
}
