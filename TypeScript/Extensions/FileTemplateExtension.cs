using KY.Generator.Templates;

namespace KY.Generator.TypeScript.Extensions
{
    public static class FileTemplateExtension
    {
        public static UsingTemplate AddUsing(this FileTemplate fileTemplate, string type, string path)
        {
            UsingTemplate usingTemplate = new UsingTemplate(null, type, path);
            fileTemplate.Usings.Add(usingTemplate);
            return usingTemplate;
        }

        public static FileTemplate WithUsing(this FileTemplate fileTemplate, string type, string path)
        {
            fileTemplate.AddUsing(type, path);
            return fileTemplate;
        }

        public static ExportTemplate AddExport(this FileTemplate fileTemplate, string type, string path)
        {
            ExportTemplate usingTemplate = new(null, type, path);
            fileTemplate.Usings.Add(usingTemplate);
            return usingTemplate;
        }

        public static FileTemplate WithExport(this FileTemplate fileTemplate, string type, string path)
        {
            fileTemplate.AddExport(type, path);
            return fileTemplate;
        }
    }
}
