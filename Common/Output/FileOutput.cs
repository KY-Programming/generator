using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;
using Environment = System.Environment;

namespace KY.Generator.Output
{
    public class FileOutput : IOutput
    {
        private readonly IEnvironment environment;
        private readonly List<IOutputAction> actions = new();
        public string BasePath { get; private set; }

        public FileOutput(IEnvironment environment, string basePath)
        {
            this.environment = environment;
            this.BasePath = basePath;
            this.environment.OutputPath = basePath;
        }

        public void Write(string fileName, string content, bool ignoreOutputId = false, bool forceOverwrite = false)
        {
            string filePath = this.ToFilePath(fileName);
            this.RemovePreviousActions(filePath);
            lock (this.actions)
            {
                this.actions.Add(new OutputWriteAction(filePath, content, ignoreOutputId ? null : this.environment.OutputId, forceOverwrite));
            }
        }

        public void Delete(string fileName)
        {
            string filePath = this.ToFilePath(fileName);
            this.RemovePreviousActions(filePath);
            lock (this.actions)
            {
                this.actions.Add(new OutputDeleteAction(filePath));
            }
        }

        public void RemoveOutputId(string filePath, string fileContent = null)
        {
            this.RemovePreviousActions(filePath);
            lock (this.actions)
            {
                this.actions.Add(new OutputRemoveIdAction(filePath, this.environment.OutputId, fileContent));
            }
        }

        public void Execute()
        {
            lock (this.actions)
            {
                this.actions.ForEach(action => action.Execute());
            }
        }

        public void DeleteAllRelatedFiles(string relativePath = null)
        {
            if (this.environment.OutputId == Guid.Empty)
            {
                return;
            }
            try
            {
                string path = this.ToFilePath(relativePath);
                if (!FileSystem.DirectoryExists(path))
                {
                    return;
                }
                List<IOutputAction> currentActions;
                lock (this.actions)
                {
                    currentActions = this.actions.ToList();
                }
                IEnumerable<string> filesToCheck = FileSystem.GetFiles(path, null, SearchOption.AllDirectories)
                                                             .Where(file => currentActions.All(action => !action.FilePath.Equals(file, StringComparison.CurrentCultureIgnoreCase)));
                foreach (string file in filesToCheck)
                {
                    string content = FileSystem.ReadAllText(file);
                    List<Guid> outputIds = OutputFileHelper.GetOutputIds(content).ToList();
                    if (!outputIds.Contains(this.environment.OutputId))
                    {
                        continue;
                    }
                    if (outputIds.Count == 1)
                    {
                        this.Delete(file);
                    }
                    else
                    {
                        this.RemoveOutputId(file, content);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Warning($"Obsolete generated file check gots an {exception.GetType().Name}. {exception.Message}{Environment.NewLine}{exception.StackTrace}");
            }
        }

        public void Move(string path)
        {
            this.BasePath = FileSystem.IsAbsolute(path) ? path : FileSystem.Combine(this.BasePath, path);
            this.environment.OutputPath = path;
        }

        private void RemovePreviousActions(string fileName)
        {
            lock (this.actions)
            {
                this.actions.RemoveAll(action => action.FilePath.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        private string ToFilePath(string fileName)
        {
            return fileName == null ? this.BasePath : FileSystem.IsAbsolute(fileName) ? fileName : FileSystem.Combine(this.BasePath, fileName);
        }

        public override string ToString()
        {
            return this.BasePath;
        }
    }
}
