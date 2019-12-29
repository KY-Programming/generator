using System.Collections.Generic;

namespace KY.Generator.Templates.Extensions
{
    public static class FileTemplateListExtension
    {
        public static FileTemplate AddFile(this IList<FileTemplate> files, string relativePath = null, bool addHeader = true, bool checkOnOverwrite = true)
        {
            FileTemplate file = new FileTemplate(relativePath, addHeader, checkOnOverwrite);
            files.Add(file);
            return file;
        }
    }
}