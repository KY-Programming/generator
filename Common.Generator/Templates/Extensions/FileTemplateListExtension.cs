namespace KY.Generator.Templates.Extensions;

public static class FileTemplateListExtension
{
    public static FileTemplate AddFile(this IList<FileTemplate> files, string? relativePath, GeneratorOptions options)
    {
        FileTemplate file = new(relativePath, options);
        files.Add(file);
        return file;
    }
}
