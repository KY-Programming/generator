using System.Collections.Generic;

namespace KY.Generator.Templates.Extensions
{
    public static class FileTemplateListExtension
    {
        public static FileTemplate AddFile(this IList<FileTemplate> files, string relativePath = null, bool addHeader = true)
        {
            FileTemplate file = new FileTemplate(relativePath, addHeader);
            files.Add(file);
            return file;
        }
    }
}