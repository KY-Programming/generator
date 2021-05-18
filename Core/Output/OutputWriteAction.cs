﻿using System;
using System.Collections.Generic;
using System.Text;
using KY.Core;
using KY.Core.DataAccess;

namespace KY.Generator.Output
{
    public class OutputWriteAction : IOutputAction
    {
        private bool executed;
        public string FilePath { get; }
        public string Content { get; }
        public Guid? OutputId { get; }

        public OutputWriteAction(string filePath, string content, Guid? outputId)
        {
            this.FilePath = filePath;
            this.Content = content;
            this.OutputId = outputId;
        }

        public void Execute()
        {
            if (this.executed)
            {
                return;
            }
            this.executed = true;
            FileSystem.CreateDirectory(FileSystem.Parent(this.FilePath));
            if (!FileSystem.FileExists(this.FilePath))
            {
                Logger.Trace($"Write file {this.FilePath}");
                this.Write(this.Content, new List<Guid>());
                return;
            }
            string contentToWriteHash = OutputFileHelper.GetHash(this.Content);
            string contentRead = FileSystem.ReadAllText(this.FilePath);
            List<Guid> readOutputIds = OutputFileHelper.GetOutputIds(contentRead);
            string contentReadHash = OutputFileHelper.GetHash(contentRead);
            bool contentReadIsGenerated = OutputFileHelper.IsGeneratedFile(contentRead);
            if (contentToWriteHash == contentReadHash)
            {
                if (this.OutputId != null && !readOutputIds.Contains(this.OutputId.Value))
                {
                    Logger.Trace($"File has no changes {this.FilePath}. Output id appended.");
                    this.Write(this.Content, readOutputIds);
                }
                else if (!contentRead.Contains(this.GetType().Assembly.GetName().Version.ToString()))
                {
                    Logger.Trace($"File has no changes {this.FilePath}. Version updated.");
                    this.Write(this.Content, readOutputIds);
                }
                else
                {
                    Logger.Trace($"File has no changes {this.FilePath}");
                }
            }
            else if (contentRead.Length == 0
                     || contentReadIsGenerated && (this.OutputId == null || readOutputIds.Count == 0 || readOutputIds.Contains(this.OutputId.Value))
                     || !contentReadIsGenerated && this.OutputId != null && readOutputIds.Contains(this.OutputId.Value)
            )
            {
                Logger.Trace($"Overwrite file {this.FilePath}");
                this.Write(this.Content, readOutputIds);
            }
            else if (!contentReadIsGenerated)
            {
                Logger.Error($"Can not overwrite file {this.FilePath}. File to overwrite is not a generated. Please delete file manually!");
            }
            else
            {
                Logger.Error($"Can not overwrite file {this.FilePath}. File to overwrite was generated by a different project. Please delete file manually!");
            }
        }

        private void Write(string content, IEnumerable<Guid> readOutputIds)
        {
            content = OutputFileHelper.AppendOutputIds(content, readOutputIds, this.OutputId);
            FileSystem.WriteAllText(this.FilePath, content, Encoding.UTF8);
        }
    }
}