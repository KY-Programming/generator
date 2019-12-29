using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KY.Generator.Properties;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("File {Name ?? \"No namespace\"}")]
    public class FileTemplate : ICodeFragment
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
        public bool CheckOnOverwrite { get; }

        public FileTemplate(string relativePath = null, bool addHeader = true, bool checkOnOverwrite = true)
        {
            this.CheckOnOverwrite = checkOnOverwrite;
            this.RelativePath = relativePath ?? string.Empty;
            this.Namespaces = new List<NamespaceTemplate>();
            this.Header = new CommentTemplate(addHeader ? Resources.Header : null);
        }

        public IEnumerable<UsingTemplate> GetUsingsByNamespace()
        {
            List<UsingTemplate> usings = new List<UsingTemplate>();
            foreach (NamespaceTemplate namespaceTemplate in this.Namespaces)
            {
                foreach (INamespaceChildren namespaceChildren in namespaceTemplate.Children)
                {
                    foreach (UsingTemplate usingTemplate in this.GetUsings(namespaceChildren))
                    {
                        if (namespaceTemplate.Name != usingTemplate.Namespace && usings.All(x => x.Namespace != usingTemplate.Namespace))
                        {
                            usings.Add(usingTemplate);
                        }
                    }
                }
            }
            return usings.OrderBy(x => x.Namespace, new NamespaceComparer());
        }

        public IEnumerable<UsingTemplate> GetUsingsByTypeAndPath()
        {
            List<UsingTemplate> usings = new List<UsingTemplate>();
            foreach (NamespaceTemplate namespaceTemplate in this.Namespaces)
            {
                foreach (INamespaceChildren namespaceChildren in namespaceTemplate.Children)
                {
                    foreach (UsingTemplate usingTemplate in this.GetUsings(namespaceChildren))
                    {
                        if (usings.All(x => x.Path != usingTemplate.Path || x.Type != usingTemplate.Type))
                        {
                            usings.Add(usingTemplate);
                        }
                    }
                }
            }
            return usings.OrderBy(x => $"{x.Path}/{x.Type}");
        }

        private IEnumerable<UsingTemplate> GetUsings(INamespaceChildren namespaceChildren)
        {
            foreach (UsingTemplate usingTemplate in namespaceChildren.Usings)
            {
                yield return usingTemplate;
            }
            if (namespaceChildren is ClassTemplate classTemplate)
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

        private class NamespaceComparer : IComparer<string>
        {
            public int Compare(string left, string right)
            {
                bool leftSystem = left != null && left.StartsWith("System");
                bool rightSystem = right != null && right.StartsWith("System");
                if (leftSystem == rightSystem)
                {
                    return StringComparer.CurrentCulture.Compare(left, right);
                }
                return leftSystem ? -1 : 1;
            }
        }
    }
}