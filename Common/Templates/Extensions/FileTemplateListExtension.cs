using System.Collections.Generic;

namespace KY.Generator.Templates.Extensions
{
    public static class FileTemplateListExtension
    {
        public static FileTemplate AddFile(this IList<FileTemplate> files, string relativePath, IOptions options)
        {
            FileTemplate file = new(relativePath, options);
            files.Add(file);
            return file;
        }
    }
}
