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

        public StaticFileTemplate(string name, byte[] content, string relativePath = null, bool addHeader = true, bool checkOnOverwrite = true)
            : this(name, Encoding.UTF8.GetString(content), relativePath, addHeader, checkOnOverwrite)
        { }

        public StaticFileTemplate(string name, string content, string relativePath = null, bool addHeader = true, bool checkOnOverwrite = true)
            : base(relativePath, addHeader, checkOnOverwrite)
        {
            this.Name = name;
            this.Content = content;
        }

        public void Write(IOutput output)
        {
            StaticFileLanguage.Instance.Write(this, output);
        }
    }
}