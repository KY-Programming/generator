using System.Collections.Generic;

namespace KY.Generator.Templates.Extensions
{
    public static class FileTemplateListExtension
    {
        public static FileTemplate AddFile(this IList<FileTemplate> files, string relativePath = null)
        {
            FileTemplate file = new FileTemplate(relativePath);
            files.Add(file);
            return file;
        }
    }
}