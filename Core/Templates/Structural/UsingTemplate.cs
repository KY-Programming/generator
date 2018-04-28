using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("Using {Namespace}")]
    public class UsingTemplate : CodeFragment
    {
        public string Namespace { get; }
        public string Type { get; }
        public string Path { get; }

        public UsingTemplate(string nameSpace, string type, string path)
        {
            this.Namespace = nameSpace;
            this.Type = type;
            this.Path = path;
        }
    }
}