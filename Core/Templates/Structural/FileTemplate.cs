using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("File {Name ?? \"No namespace\"}")]
    public class FileTemplate
    {
        private string name;

        public string Name
        {
            get => this.name ?? this.Namespaces.FirstOrDefault()?.Children.FirstOrDefault()?.Name;
            set => this.name = value;
        }

        public string RelativePath { get; }
        public List<NamespaceTemplate> Namespaces { get; }
        public CommentTemplate Header { get; }

        public FileTemplate(string relativePath = null)
        {
            this.RelativePath = relativePath ?? string.Empty;
            this.Namespaces = new List<NamespaceTemplate>();
            this.Header = new CommentTemplate();
        }

        public IEnumerable<UsingTemplate> GetUsingsByNamespace()
        {
            List<string> namespaces = new List<string>();
            foreach (NamespaceTemplate namespaceTemplate in this.Namespaces)
            {
                foreach (INamespaceChildren namespaceChildren in namespaceTemplate.Children)
                {
                    foreach (UsingTemplate usingTemplate in this.GetUsings(namespaceChildren))
                    {
                        if (namespaceTemplate.Name != usingTemplate.Namespace && !namespaces.Contains(usingTemplate.Namespace))
                        {
                            yield return usingTemplate;
                            namespaces.Add(usingTemplate.Namespace);
                        }
                    }
                }
            }
        }

        public IEnumerable<UsingTemplate> GetUsingsByTypeAndPath()
        {
            List<string> paths = new List<string>();
            foreach (NamespaceTemplate namespaceTemplate in this.Namespaces)
            {
                foreach (INamespaceChildren namespaceChildren in namespaceTemplate.Children)
                {
                    foreach (UsingTemplate usingTemplate in this.GetUsings(namespaceChildren))
                    {
                        string path = usingTemplate.Path + usingTemplate.Type;
                        if (!paths.Contains(path))
                        {
                            yield return usingTemplate;
                            paths.Add(path);
                        }
                    }
                }
            }
        }

        private IEnumerable<UsingTemplate> GetUsings(INamespaceChildren namespaceChildren)
        {
            foreach (UsingTemplate usingTemplate in namespaceChildren.Usings)
            {
                yield return usingTemplate;
            }
            ClassTemplate classTemplate = namespaceChildren as ClassTemplate;
            if (classTemplate != null)
            {
                foreach (ClassTemplate subclassTemplate in classTemplate.Classes)
                {
                    foreach (UsingTemplate usingTemplate in this.GetUsings(subclassTemplate))
                    {
                        yield return usingTemplate;
                    }
                }
            }
        }
    }
}