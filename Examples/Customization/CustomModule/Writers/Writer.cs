using System.Collections.Generic;
using CustomModule.Commands;
using KY.Core;
using KY.Generator;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace CustomModule.Writers
{
    // 6. Create a Writer
    //  a. Implement KY.Generator.Transfer.Writers.ITransferWriter
    //  b. To use code writing features derive from KY.Generator.Codeable
    internal class Writer : Codeable
    {
        public void Write(WriteCommandParameters parameters, List<FileTemplate> files)
        {
            Logger.Trace($"Write a class for \"{parameters.Message}\"...");
            ClassTemplate classTemplate = files.AddFile(parameters.RelativePath, !parameters.SkipHeader, null)
                                               .AddNamespace(parameters.Namespace)
                                               .AddClass(parameters.Class)
                /*.FormatName(parameters.FormatNames)*/;
            classTemplate.WithUsing("System", null, null)
                         .AddMethod("Write", Code.Void())
                         .Code.AddLine(Code.Static(Code.Type("Console")).Method("WriteLine", Code.String(parameters.Message)).Close());
        }
    }
}