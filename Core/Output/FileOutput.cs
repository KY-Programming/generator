using System.Text;
using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator.Output
{
    public class FileOutput : IOutput
    {
        private readonly string basePath;

        public FileOutput(string basePath)
        {
            this.basePath = basePath;
        }

        public void Write(string fileName, string content)
        {
            string filePath = FileSystem.Combine(this.basePath, fileName.Trim('\\'));
            FileSystem.CreateDirectory(FileSystem.Parent(filePath));
            if (!FileSystem.FileExists(filePath) || FileSystem.ReadAllText(filePath) != content)
            {
                Logger.Trace($"Write file {filePath}");
                FileSystem.WriteAllText(filePath, content, Encoding.UTF8);
            }
            else
            {
                Logger.Trace($"File has no changes {filePath}");
            }
        }

        public override string ToString()
        {
            return this.basePath;
        }
    }
}