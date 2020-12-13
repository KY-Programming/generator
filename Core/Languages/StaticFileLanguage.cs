using System;
using System.Collections.Generic;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Languages
{
    internal class StaticFileLanguage : EmptyFormattableLanguage, ITemplateWriter
    {
        public static StaticFileLanguage Instance { get; } = new StaticFileLanguage();

        public override string Name => nameof(StaticFileLanguage);

        private StaticFileLanguage()
        {
        }

        public override void Write(FileTemplate fileTemplate, IOutput output)
        {
            if (string.IsNullOrEmpty(fileTemplate.Name))
            {
                Logger.Trace("Empty file skipped");
                return;
            }
            if (fileTemplate.Header.Description != null)
            {
                AssemblyName assemblyName = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName();
                fileTemplate.Header.Description = string.Format(fileTemplate.Header.Description, $"{assemblyName.Name} {assemblyName.Version}");
            }
            StaticFileTemplate staticFile = fileTemplate as StaticFileTemplate;
            if (staticFile == null)
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {fileTemplate.GetType().Name} is not implemented in {this.Name}.");
            }
            
            FileWriter fileWriter = new FileWriter(this);
            fileWriter.Add(fileTemplate.Header)
                      .BreakLine()
                      .Add(staticFile.Content, true)
                      .BreakLine()
                      .Add(fileTemplate.OutputIdComment);
            
            string fileName = FileSystem.Combine(fileTemplate.RelativePath, fileTemplate.Name);
            output.Write(fileName, fileWriter.ToString(), fileTemplate.OutputId);
        }

        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            if (fragment is CommentTemplate)
            {
                new CommentWriter().Write(fragment, output);
            }
            else
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {fragment.GetType().Name} is not implemented in {this.Name}.");
            }
        }

        public override void Write<T>(IEnumerable<T> code, IOutputCache output)
        {
            code.ForEach(fragment => this.Write(fragment, output));
        }
    }
}