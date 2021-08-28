using System;
using System.Diagnostics;
using System.Text;
using KY.Generator.Languages;
using KY.Generator.Output;

namespace KY.Generator.Templates
{
    [DebuggerDisplay("StaticFile {Name}")]
    public class StaticFileTemplate : FileTemplate
    {
        public string Content { get; }

        public StaticFileTemplate(string name, byte[] content, string relativePath, IOptions options)
            : this(name, Encoding.UTF8.GetString(content), relativePath, options)
        { }

        public StaticFileTemplate(string name, string content, string relativePath, IOptions options)
            : base(relativePath, options)
        {
            this.Name = name;
            this.Content = content;
        }
    }
}
