using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using KY.Core;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Writers;
using KY.Generator.TypeScript.Extensions;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript.Transfer.Writers
{
    public class TypeScriptIndexWriter : TransferWriter
    {
        private readonly List<ITransferObject> transferObjects;
        private readonly List<FileTemplate> files;

        public TypeScriptIndexWriter(ITypeMapping typeMapping, Options options, List<ITransferObject> transferObjects, List<FileTemplate> files)
            : base(typeMapping, options)
        {
            this.transferObjects = transferObjects;
            this.files = files;
        }

        public virtual void Write()
        {
            Logger.Trace("Generate index.ts...");
            foreach (TypeScriptIndexFile file in this.transferObjects.OfType<TypeScriptIndexFile>())
            {
                this.Write(file, file.RelativePath);
            }
        }

        public virtual void Write(TypeScriptIndexFile file, string relativePath)
        {
            if (file == null)
            {
                return;
            }
            FileTemplate fileTemplate = this.files.AddFile(relativePath, this.Options.Current)
                                            .WithName(Formatter.FormatFile("index", this.Options.Current))
                                            .IgnoreOutputId()
                                            .ForceOverwrite()
                                            .SkipHeader();
            fileTemplate.Linters = new Dictionary<string, bool>();
            foreach (IIndexLine line in file.Lines)
            {
                if (line is ExportIndexLine indexLine)
                {
                    foreach (string type in indexLine.Types)
                    {
                        fileTemplate.AddExport(type, indexLine.Path.TrimEnd(".ts"));
                    }
                }
                else if (line is UnknownIndexLine unknownIndexLine)
                {
                    fileTemplate.Usings.Add(new UnknownExportTemplate(Code.TypeScript(unknownIndexLine.Content)));
                }
            }
            foreach (ExportTemplate exportTemplate in fileTemplate.Usings.OfType<ExportTemplate>().ToList())
            {
                if (fileTemplate.Usings.Count(u => u.Type == exportTemplate.Type && u.Path.Equals(exportTemplate.Path, StringComparison.InvariantCultureIgnoreCase)) > 1)
                {
                    fileTemplate.Usings.Remove(exportTemplate);
                }
            }
        }
    }
}
