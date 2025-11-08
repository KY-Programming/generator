using System.Diagnostics;
using KY.Generator.Properties;

namespace KY.Generator.Templates;

[DebuggerDisplay("File {Name ?? \"No namespace\"}")]
public class FileTemplate : ICodeFragment
{
    public GeneratorOptions Options { get; }
    private string? name;

    public string? Name
    {
        get => this.name ?? this.Namespaces.FirstOrDefault(x => x.Children.Any(y => y.IsPublic))?.Children.FirstOrDefault(x => x.IsPublic)?.Name;
        set => this.name = value;
    }

    public string? FullPath { get; set; }
    public string? RelativePath { get; }
    public List<NamespaceTemplate> Namespaces { get; } = [];
    public CommentTemplate Header { get; }
    public List<UsingTemplate> Usings { get; } = [];
    public bool WriteOutputId { get; set; } = true;
    public bool ForceOverwrite { get; set; }
    public Dictionary<string, bool> Linters { get; set; }

    public FileTemplate(string? relativePath, GeneratorOptions options)
    {
        this.RelativePath = relativePath ?? string.Empty;
        this.Options = options;
        this.Header = new CommentTemplate(options.AddHeader ? Resources.Header : null);
    }

    public IEnumerable<UsingTemplate> GetUsingsByNamespace()
    {
        List<UsingTemplate> usings = this.Usings.ToList();
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
        List<UsingTemplate> usings = this.Usings.ToList();
        foreach (NamespaceTemplate namespaceTemplate in this.Namespaces)
        {
            foreach (INamespaceChildren namespaceChildren in namespaceTemplate.Children)
            {
                foreach (UsingTemplate usingTemplate in this.GetUsings(namespaceChildren))
                {
                    if (namespaceTemplate.Children.Any(x => x.Name == usingTemplate.Type))
                    {
                        continue;
                    }
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
        foreach (UsingTemplate usingTemplate in this.Usings)
        {
            yield return usingTemplate;
        }
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
