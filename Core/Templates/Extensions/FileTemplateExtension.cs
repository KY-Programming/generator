namespace KY.Generator.Templates.Extensions
{
    public static class FileTemplateExtension
    {
        public static FileTemplate WithHeader(this FileTemplate file, string header)
        {
            file.Header.Description = header;
            return file;
        }

        public static NamespaceTemplate AddNamespace(this FileTemplate file, string nameSpace)
        {
            NamespaceTemplate namespaceTemplate = new NamespaceTemplate(file, nameSpace);
            file.Namespaces.Add(namespaceTemplate);
            return namespaceTemplate;
        }
    }
}