using System.Collections.Generic;
using KY.Generator.Configuration;
using KY.Generator.Templates;

namespace KY.Generator.Extensions
{
    public static class FileTemplateListExtension
    {
        public static void Write(this List<FileTemplate> files, IConfigurationWithLanguage configuration)
        {
            files.ForEach(file => configuration.Language.Write(file, configuration.Output));
        }
    }
}