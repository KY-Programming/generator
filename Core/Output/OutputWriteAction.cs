using System.Text;
using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator.Output
{
    public class OutputWriteAction : IOutputAction
    {
        public string FilePath { get; }
        public string Content { get; }

        public OutputWriteAction(string filePath, string content)
        {
            this.FilePath = filePath;
            this.Content = content;
        }

        public void Execute()
        {
            FileSystem.CreateDirectory(FileSystem.Parent(this.FilePath));
            if (!FileSystem.FileExists(this.FilePath) || FileSystem.ReadAllText(this.FilePath) != this.Content)
            {
                Logger.Trace($"Write file {this.FilePath}");
                FileSystem.WriteAllText(this.FilePath, this.Content, Encoding.UTF8);
            }
            else
            {
                Logger.Trace($"File has no changes {this.FilePath}");
            }
        }
    }
}