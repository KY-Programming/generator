using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    //class FileWriter : ITemplateWriter
    //{
    //    public virtual void Write(ICodeFragment fragment, IOutputCache output)
    //    {
    //        FileTemplate fileTemplate = (FileTemplate)fragment;
            
    //        if (string.IsNullOrEmpty(fileTemplate.Name))
    //        {
    //            Logger.Trace("Empty file skipped");
    //            return;
    //        }
    //        if (fileTemplate.Header.Description != null)
    //        {
    //            AssemblyName assemblyName = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName();
    //            fileTemplate.Header.Description = string.Format(fileTemplate.Header.Description, $"{assemblyName.Name} {assemblyName.Version}");
    //        }
    //        IMetaElementList elements = new MetaElementList();
    //        StaticFileTemplate staticFile = fileTemplate as StaticFileTemplate;
    //        if (staticFile == null)
    //        {
    //            string fileName = FileSystem.Combine(fileTemplate.RelativePath, this.GetFileName(fileTemplate));
    //            this.WriteHeader(elements, fileTemplate);
    //            this.WriteUsings(elements, fileTemplate);
    //            this.Write(elements, fileTemplate.Namespaces);
    //            FileWriter fileWriter = new FileWriter(output, fileName);
    //            MetaGenerator metaGenerator = new MetaGenerator(fileWriter, this.Formatting);
    //            metaGenerator.Write(elements);
    //            fileWriter.WriteFile();
    //        }
    //        else
    //        {
    //            string fileName = FileSystem.Combine(fileTemplate.RelativePath, fileTemplate.Name);
    //            this.WriteHeader(elements, fileTemplate);
    //            FileWriter fileWriter = new FileWriter(output, fileName);
    //            MetaGenerator metaGenerator = new MetaGenerator(fileWriter, this.Formatting);
    //            metaGenerator.Write(elements);
    //            fileWriter.Append(staticFile.Content);
    //            if (this.Formatting.EndFileWithNewLine)
    //            {
    //                fileWriter.AppendLine();
    //            }
    //            fileWriter.WriteFile();
    //        }
    //    }
    //}
}
