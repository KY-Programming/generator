using System;
using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator.Output
{
    public class OutputRemoveIdAction : IOutputAction
    {
        private bool executed;
        public string FilePath { get; }
        public Guid OutputId { get; }
        public string FileContent { get; }

        public OutputRemoveIdAction(string fileName, Guid outputId, string fileContent = null)
        {
            this.FilePath = fileName;
            this.OutputId = outputId;
            this.FileContent = fileContent;
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
                Logger.Trace($"Remove output id from {this.FilePath}");
                string content = this.FileContent ?? FileSystem.ReadAllText(this.FilePath);
                content = OutputFileHelper.RemoveOutputId(content, this.OutputId);
                FileSystem.WriteAllText(this.FilePath, content);
            }
        }
    }
}