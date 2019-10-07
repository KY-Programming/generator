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
            Logger.Trace($"Write file {filePath}");
            FileSystem.CreateDirectory(FileSystem.Parent(filePath));
            FileSystem.WriteAllText(filePath, content, Encoding.UTF8);
        }

        public override string ToString()
        {
            return this.basePath;
        }
    }
}