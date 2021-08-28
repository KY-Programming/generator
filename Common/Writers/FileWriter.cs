using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Writers
{
    public class FileWriter : ITemplateWriter
    {
        private readonly IOptions options;

        public FileWriter(IOptions options)
        {
            this.options = options;
        }

        public virtual void Write(ICodeFragment fragment, IOutputCache output)
        {
            FileTemplate template = (FileTemplate)fragment;
            if (string.IsNullOrEmpty(template.Name))
            {
                Logger.Trace("Empty file skipped");
                return;
            }
            if (template.Header.Description != null)
            {
                AssemblyName assemblyName = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName();
                template.Header.Description = string.Format(template.Header.Description, $"{assemblyName.Name} {assemblyName.Version}");
            }
            template.FullPath = FileSystem.Combine(template.RelativePath, template.Name);
            this.WriteHeader(template, output);
            if (template is StaticFileTemplate staticFile)
            {
                output.Add(staticFile.Content, true);
            }
            else
            {
                this.WriteUsings(template, output);
                output.Add(template.Namespaces);
            }
        }

        protected virtual void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            if (fileTemplate.Header?.Description != null)
            {
                output.Add(fileTemplate.Header)
                      .BreakLine();
            }
        }

        protected virtual void WriteUsings(FileTemplate fileTemplate, IOutputCache output)
        {
            List<UsingTemplate> usings = this.GetUsings(fileTemplate).ToList();
            if (usings.Count <= 0)
            {
                return;
            }
            output.Add(usings)
                  .BreakLine();
        }

        protected virtual IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByNamespace();
        }
    }
}
