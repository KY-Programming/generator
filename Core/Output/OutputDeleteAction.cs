using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator.Output
{
    public class OutputDeleteAction : IOutputAction
    {
        private bool executed;
        public string FilePath { get; }

        public OutputDeleteAction(string fileName)
        {
            this.FilePath = fileName;
        }

        public void Execute()
        {
            if (this.executed)
            {
                return;
            }
            this.executed = true;
            if (FileSystem.FileExists(this.FilePath))
            {
                Logger.Trace($"Delete file {this.FilePath}");
                FileSystem.DeleteFile(this.FilePath);
            }
        }
    }
}