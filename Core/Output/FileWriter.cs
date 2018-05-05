using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core.Meta;

namespace KY.Generator.Output
{
    public class FileWriter : IMetaFileWriter
    {
        private readonly IOutput output;
        private readonly List<FileWriterLine> lines;

        public string FileName { get; }
        public FileWriterLine LastLine => this.lines.LastOrDefault();

        public FileWriter(IOutput output, string fileName)
        {
            this.FileName = fileName;
            this.output = output;
            this.lines = new List<FileWriterLine>();
        }

        public IMetaFileWriter Append(object value)
        {
            string valueString = value?.ToString();
            if (string.IsNullOrEmpty(valueString))
            {
                return this;
            }
            FileWriterLine lastLine = this.LastLine;
            if (lastLine == null)
            {
                lastLine = new FileWriterLine();
                this.lines.Add(lastLine);
                return this.Append(value);
            }
            this.LastLine.Content += value;
            return this;
        }

        public IMetaFileWriter AppendLine()
        {
            this.lines.Add(new FileWriterLine());
            return this;
        }

        public IMetaFileWriter AppendLine(object value)
        {
            return this.Append(value)
                       .AppendLine();
        }

        public void WriteFile()
        {
            if (this.lines.Count > 0 && !string.IsNullOrWhiteSpace(this.FileName))
            {
                this.TrimEnd();
                this.output.Write(this.FileName, string.Join(Environment.NewLine, this.lines));
            }
            this.lines.Clear();
        }

        public IMetaFileWriter TrimEnd()
        {
            while (string.IsNullOrEmpty(this.LastLine?.Content) && this.lines.Count > 0)
            {
                this.lines.Remove(this.LastLine);
            }
            return this;
        }
    }
}