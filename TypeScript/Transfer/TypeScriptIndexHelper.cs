using System.Collections.Generic;
using System.Linq;
using KY.Core.DataAccess;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Transfer.Readers;
using KY.Generator.TypeScript.Transfer.Writers;

namespace KY.Generator.TypeScript.Transfer;

public class TypeScriptIndexHelper
{
    private readonly TypeScriptIndexReader reader;
    private readonly TypeScriptIndexWriter writer;
    private readonly List<FileTemplate> files;
    private readonly TypeScriptOptions options;

    public TypeScriptIndexHelper(TypeScriptIndexReader reader, TypeScriptIndexWriter writer, List<FileTemplate> files, Options options)
    {
        this.reader = reader;
        this.writer = writer;
        this.files = files;
        this.options = options.Get<TypeScriptOptions>();
    }

    public void Execute(string? relativePath)
    {
        if (this.options.NoIndex && !this.options.ForceIndex)
        {
            return;
        }

        TypeScriptIndexFile? indexFile = this.reader.Read(relativePath);

        List<FileTemplate> fileTemplates = this.files.Where(file => file.RelativePath == relativePath
                                                                    && file.Name != "index.ts"
                                                                    && file.Options.Language.IsTypeScript()
        ).ToList();
        if ((fileTemplates.Count > 1 || this.options.ForceIndex) && indexFile == null)
        {
            indexFile = new TypeScriptIndexFile();
        }
        if (indexFile != null)
        {
            foreach (FileTemplate file in fileTemplates)
            {
                string shortenedRelativePath = FileSystem.Combine(".", FileSystem.RelativeTo(FileSystem.Combine(file.RelativePath, file.Name), relativePath));
                indexFile.Lines.Add(new ExportIndexLine { Path = shortenedRelativePath.Replace("\\", "/"), Types = { "*" } });
            }
        }

        this.writer.Write(indexFile, relativePath);
    }
}