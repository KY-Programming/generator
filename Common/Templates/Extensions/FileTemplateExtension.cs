using System.Collections.Generic;
using KY.Generator.Transfer;

namespace KY.Generator.Templates.Extensions
{
    public static class FileTemplateExtension
    {
        public static FileTemplate WithName(this FileTemplate file, string name)
        {
            file.Name = name;
            return file;
        }

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

        public static void AddTo(this FileTemplate file, IList<FileTemplate> list)
        {
            list.Add(file);
        }

        public static UsingTemplate AddUsing(this FileTemplate fileTemplate, string nameSpace, string type, string path)
        {
            return fileTemplate.AddUsing(new UsingTemplate(nameSpace, type, path));
        }

        public static UsingTemplate AddUsing(this FileTemplate fileTemplate, TypeTransferObject type, string path)
        {
            return fileTemplate.AddUsing(new LinkedUsingTemplate(type, path));
        }

        public static UsingTemplate AddUsing(this FileTemplate fileTemplate, UsingTemplate usingTemplate)
        {
            fileTemplate.Usings.Add(usingTemplate);
            return usingTemplate;
        }

        public static FileTemplate WithUsing(this FileTemplate fileTemplate, UsingTemplate usingTemplate)
        {
            fileTemplate.Usings.Add(usingTemplate);
            return fileTemplate;
        }

        public static FileTemplate WithUsing(this FileTemplate fileTemplate, string nameSpace, string type, string path)
        {
            fileTemplate.AddUsing(nameSpace, type, path);
            return fileTemplate;
        }

        public static FileTemplate IgnoreOutputId(this FileTemplate fileTemplate)
        {
            fileTemplate.WriteOutputId = false;
            return fileTemplate;
        }

        public static FileTemplate ForceOverwrite(this FileTemplate fileTemplate)
        {
            fileTemplate.ForceOverwrite = true;
            return fileTemplate;
        }

        public static FileTemplate SkipHeader(this FileTemplate fileTemplate)
        {
            fileTemplate.Header.Description = null;
            return fileTemplate;
        }
    }
}
