using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KY.Core.DataAccess;

namespace KY.Generator.Output
{
    public class FileOutput : IOutput
    {
        private readonly List<IOutputAction> actions;
        private string basePath;

        public FileOutput(string basePath)
        {
            this.actions = new List<IOutputAction>();
            this.basePath = basePath;
        }

        public void Write(string fileName, string content, Guid? outputId)
        {
            string filePath = this.ToFilePath(fileName);
            this.RemovePreviousActions(filePath);
            this.actions.Add(new OutputWriteAction(filePath, content, outputId));
        }

        public void Delete(string fileName)
        {
            string filePath = this.ToFilePath(fileName);
            this.RemovePreviousActions(filePath);
            this.actions.Add(new OutputDeleteAction(filePath));
        }

        public void Execute()
        {
            this.actions.ForEach(action => action.Execute());
        }

        public void DeleteAllRelatedFiles(Guid? outputId, string relativePath = null)
        {
            if (outputId == null)
            {
                return;
            }
            string path = this.ToFilePath(relativePath);
            if (!FileSystem.DirectoryExists(relativePath))
            {
                return;
            }
            IEnumerable<string> filesToCheck = FileSystem.GetFiles(path, null, SearchOption.AllDirectories)
                                                         .Where(file => this.actions.All(action => !action.FilePath.Equals(file, StringComparison.CurrentCultureIgnoreCase)));
            foreach (string file in filesToCheck)
            {
                if (FileSystem.ReadAllText(file).Contains($"outputid:{outputId}"))
                {
                    this.Delete(file);
                }
            }
        }

        public void Move(string path)
        {
            this.basePath = FileSystem.IsAbsolute(path) ? path : FileSystem.Combine(this.basePath, path);
        }

        private void RemovePreviousActions(string fileName)
        {
            this.actions.RemoveAll(action => action.FilePath.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));
        }

        private string ToFilePath(string fileName)
        {
            return fileName == null ? this.basePath : FileSystem.IsAbsolute(fileName) ? fileName : FileSystem.Combine(this.basePath, fileName);
        }

        public override string ToString()
        {
            return this.basePath;
        }
    }
}