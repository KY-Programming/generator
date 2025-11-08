using System.Diagnostics;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("Using {Namespace} {Type}")]
    public class UsingTemplate : ICodeFragment
    {
        public virtual string Namespace { get; }
        public virtual string Type { get; }
        public virtual string Path { get; }

        public UsingTemplate(string nameSpace, string type, string path)
        {
            this.Namespace = nameSpace;
            this.Type = type;
            this.Path = path;
        }

        protected UsingTemplate()
        { }
    }
}
