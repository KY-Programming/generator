using System.Diagnostics;
using System.Text;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("StaticFile {Name}")]
    public class StaticFileTemplate : FileTemplate
    {
        public string Content { get; }

        public StaticFileTemplate(string name, byte[] content, string relativePath = null, bool addHeader = true)
            : this(name, Encoding.UTF8.GetString(content), relativePath, addHeader)
        { }

        public StaticFileTemplate(string name, string content, string relativePath = null, bool addHeader = true)
            : base(relativePath, addHeader)
        {
            this.Name = name;
            this.Content = content;
        }
    }
}