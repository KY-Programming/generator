using KY.Core.DataAccess;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Transfer.Readers;
using KY.Generator.TypeScript.Transfer.Writers;

namespace KY.Generator.TypeScript.Transfer;

public class TypeScriptIndexHelper
{
    private const string indexFileName = "index.ts";
    private readonly TypeScriptIndexReader reader;
    private readonly TypeScriptIndexWriter writer;
    private readonly List<FileTemplate> files;
    private readonly Options options;
    private readonly IEnvironment environment;

    public TypeScriptIndexHelper(TypeScriptIndexReader reader, TypeScriptIndexWriter writer, List<FileTemplate> files, Options options, IEnvironment environment)
    {
        this.reader = reader;
        this.writer = writer;
        this.files = files;
        this.options = options;
        this.environment = environment;
    }

    /// <summary>
    /// Writes an index.ts for every folder that contains generated TypeScript files. The folder of a file is not
    /// necessarily the <paramref name="relativePath"/> of the command, it can also be set per model (e.g. by
    /// <see cref="GenerateModelOutputAttribute" />), so the folders are taken from the already written files.
    /// </summary>
    public void Execute(string? relativePath)
    {
        List<FileTemplate> typeScriptFiles = this.files.Where(file => file.Name != indexFileName
                                                                     && (file.Options.Language?.IsTypeScript() ?? false)
        ).ToList();

        Dictionary<string, string?> relativePaths = new();
        foreach (string? path in typeScriptFiles.Select(file => file.RelativePath).Append(relativePath))
        {
            string key = Normalize(path);
            if (!relativePaths.ContainsKey(key))
            {
                relativePaths.Add(key, path);
            }
        }

        foreach (string? path in relativePaths.Values)
        {
            this.Execute(path, typeScriptFiles);
        }
    }

    private void Execute(string? relativePath, List<FileTemplate> typeScriptFiles)
    {
        TypeScriptIndexFile? indexFile = this.reader.Read(relativePath);

        List<FileTemplate> fileTemplates = typeScriptFiles.Where(file => Normalize(file.RelativePath) == Normalize(relativePath)).ToList();

        TypeScriptOptions typeScriptOptions = this.options.Get<TypeScriptOptions>();
        List<TypeScriptOptions> fileOptions = fileTemplates.Select(x => this.options.Get<TypeScriptOptions>(x.Options)).ToList();
        bool forceIndex = typeScriptOptions.ForceIndex || fileOptions.Any(o => o.ForceIndex);
        // NoIndex suppresses the index as soon as one file requests it. All() would not work, because a folder can
        // also contain models from a referenced assembly, which does not know anything about the GenerateNoIndex
        // of the generated assembly. ForceIndex still wins over NoIndex.
        bool noIndex = typeScriptOptions.NoIndex || fileOptions.Any(o => o.NoIndex);
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
            indexFile.Lines.RemoveAll(line => this.IsObsolete(line, relativePath, typeScriptFiles));
            foreach (FileTemplate file in fileTemplates)
            {
                string fileRelativePath = FileSystem.Combine(file.RelativePath, file.Name);
                string shortenedRelativePath = FileSystem.Combine(".", string.IsNullOrEmpty(relativePath) ? fileRelativePath : FileSystem.RelativeTo(fileRelativePath, relativePath));
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

    /// <summary>
    /// An already existing index.ts is read and merged with the newly generated files, otherwise manually added
    /// exports would be lost. Exports of files that are not generated anymore have to be removed, because the file
    /// itself is deleted by the obsolete file check and the export would point to nothing.
    /// </summary>
    private bool IsObsolete(IIndexLine line, string? relativePath, List<FileTemplate> typeScriptFiles)
    {
        if (line is not ExportIndexLine exportLine)
        {
            return false;
        }
        string fileName = exportLine.Path.EndsWith(".ts") ? exportLine.Path : exportLine.Path + ".ts";
        string fullPath = FileSystem.Combine(this.environment.OutputPath, relativePath ?? string.Empty, fileName);
        if (typeScriptFiles.Any(file => string.Equals(FileSystem.Combine(this.environment.OutputPath, file.RelativePath, file.Name), fullPath, StringComparison.CurrentCultureIgnoreCase)))
        {
            return false;
        }
        if (!FileSystem.FileExists(fullPath))
        {
            return true;
        }
        // The file still exists, but if it was generated by this project, it is deleted by the obsolete file check
        return OutputFileHelper.GetOutputIds(FileSystem.ReadAllText(fullPath)).Contains(this.environment.OutputId);
    }

    private static string Normalize(string? relativePath)
    {
        return (relativePath ?? string.Empty).Replace("\\", "/").TrimEnd('/');
    }
}
