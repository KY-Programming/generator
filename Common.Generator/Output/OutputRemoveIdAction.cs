using System;
using System.Collections.Generic;
using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator.Output
{
    /// <summary>
    /// Takes the output id of this project out of a generated file, and deletes the file once no project is left that
    /// generates it
    /// </summary>
    public class OutputRemoveIdAction : IOutputAction
    {
        private bool executed;
        public string FilePath { get; }
        public Guid OutputId { get; }

        public OutputRemoveIdAction(string fileName, Guid outputId)
        {
            this.FilePath = fileName;
            this.OutputId = outputId;
        }

        public void Execute()
        {
            if (this.executed)
            {
                return;
            }
            this.executed = true;
            // Another project can add or remove its id at the same time in a parallel build (msbuild /m), so the ids are
            // read and written back under a lock that holds across processes. Read again under it - content read
            // earlier would write back a state without the other project's change, and deciding on it whether this is
            // the last id would delete a file another project just added itself to
            using IDisposable fileLock = FileLock.Acquire(this.FilePath);
            if (!FileSystem.FileExists(this.FilePath))
            {
                return;
            }
            string content = FileSystem.ReadAllText(this.FilePath);
            List<Guid> outputIds = OutputFileHelper.GetOutputIds(content);
            if (!outputIds.Contains(this.OutputId))
            {
                return;
            }
            if (outputIds.Count == 1)
            {
                Logger.Trace($"Delete file {this.FilePath}");
                FileSystem.DeleteFile(this.FilePath);
                return;
            }
            Logger.Trace($"Remove output id from {this.FilePath}");
            FileSystem.WriteAllText(this.FilePath, OutputFileHelper.RemoveOutputId(content, this.OutputId));
        }
    }
}