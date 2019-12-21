using System;
using System.Collections.Generic;
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

        public void Write(string fileName, string content)
        {
            string filePath = this.ToFilePath(fileName);
            this.RemovePreviousActions(filePath);
            this.actions.Add(new OutputWriteAction(filePath, content));
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
            return FileSystem.Combine(this.basePath, fileName.Trim('\\'));
        }
    }
}