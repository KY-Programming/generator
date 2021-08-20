using System;
using System.Collections.Generic;

namespace KY.Generator.Templates.Extensions
{
    public static class FileTemplateListExtension
    {
        public static FileTemplate AddFile(this IList<FileTemplate> files, string relativePath, bool addHeader, Guid? outputId)
        {
            FileTemplate file = new FileTemplate(relativePath, addHeader, outputId);
            files.Add(file);
            return file;
        }
    }
}