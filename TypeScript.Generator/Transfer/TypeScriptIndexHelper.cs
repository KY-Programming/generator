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
    private readonly Options options;

    public TypeScriptIndexHelper(TypeScriptIndexReader reader, TypeScriptIndexWriter writer, List<FileTemplate> files, Options options)
    {
        this.reader = reader;
        this.writer = writer;
        this.files = files;
        this.options = options;
    }

    public void Execute(string? relativePath)
    {
        TypeScriptIndexFile? indexFile = this.reader.Read(relativePath);

        List<FileTemplate> fileTemplates = this.files.Where(file => file.RelativePath == relativePath
                                                                    && file.Name != "index.ts"
                                                                    && (file.Options.Language?.IsTypeScript() ?? false)
        ).ToList();

        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();
        List<TypeScriptOptions> fileOptions = fileTemplates.Select(x => this.options.Get<TypeScriptOptions>(x.Options)).ToList();
        bool forceIndex = typeScriptOptions.ForceIndex || fileOptions.Any(o => o.ForceIndex);
        bool noIndex = typeScriptOptions.NoIndex || fileOptions.Count > 0 && fileOptions.All(o => o.NoIndex);
        if (noIndex && !forceIndex)
        {
            return;
        }

        if ((fileTemplates.Count > 1 || forceIndex) && indexFile == null)
        {
            indexFile = new TypeScriptIndexFile();
        }
        if (indexFile != null)
        {
            foreach (FileTemplate file in fileTemplates)
            {
                string shortenedRelativePath = FileSystem.Combine(".", FileSystem.RelativeTo(FileSystem.Combine(file.RelativePath, file.Name), relativePath));
                indexFile.Lines.Add(new ExportIndexLine
                {
                    Path = shortenedRelativePath.Replace("\\", "/"),
                    Types =
                    {
                        "*"
                    }
                });
            }
        }

        this.writer.Write(indexFile, relativePath);
    }
}
