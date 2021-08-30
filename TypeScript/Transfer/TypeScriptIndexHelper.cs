using System.Collections.Generic;
using System.IO;
using System.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Transfer.Readers;
using KY.Generator.TypeScript.Transfer.Writers;

namespace KY.Generator.TypeScript.Transfer
{
    public class TypeScriptIndexHelper
    {
        private readonly TypeScriptIndexReader reader;
        private readonly TypeScriptIndexWriter writer;
        private readonly List<FileTemplate> files;

        public TypeScriptIndexHelper(TypeScriptIndexReader reader, TypeScriptIndexWriter writer, List<FileTemplate> files)
        {
            this.reader = reader;
            this.writer = writer;
            this.files = files;
        }

        public void Execute(string relativePath)
        {
            TypeScriptIndexFile indexFile = this.reader.Read(relativePath);

            List<FileTemplate> fileTemplates = this.files.Where(file => file.RelativePath == relativePath && file.Name != "index.ts").ToList();
            if (fileTemplates.Count > 1 && indexFile == null)
            {
                indexFile = new TypeScriptIndexFile();
            }
            if (indexFile != null)
            {
                foreach (FileTemplate file in fileTemplates)
                {
                    string shortenedRelativePath = FileSystem.Combine(".", FileSystem.RelativeTo(FileSystem.Combine(file.RelativePath, file.Name), relativePath));
                    indexFile.Lines.Add(new ExportIndexLine { Path = shortenedRelativePath.Replace("\\", "/"), Types = { "*" } });
                }
            }

            this.writer.Write(indexFile, relativePath);
        }
    }
}
