using System.Collections.Generic;
using System.Diagnostics;

namespace KY.Generator.Templates
{
    public interface INamespaceChildren : ICodeFragment
    {
        string Name { get; }
        List<UsingTemplate> Usings { get; }
        bool IsPublic { get; }
    }

    [DebuggerDisplay("Namespace {Name}")]
    public class NamespaceTemplate : ICodeFragment
    {
        public FileTemplate File { get; }
        public string Name { get; }
        public List<INamespaceChildren> Children { get; }

        public NamespaceTemplate(FileTemplate file, string name)
        {
            this.File = file;
            this.Name = name;
            this.Children = new List<INamespaceChildren>();
        }
    }
}