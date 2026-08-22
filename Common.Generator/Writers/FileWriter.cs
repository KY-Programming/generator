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
                string generatedWith = template.Options.AddHeaderVersion ? $"{assemblyName.Name} {assemblyName.Version}" : assemblyName.Name;
                template.Header.Description = string.Format(template.Header.Description, generatedWith);
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

        /// <summary>
        /// The comment that switches the linter off for a generated file of this language. <c>null</c> for a language
        /// no linter runs on
        /// </summary>
        protected virtual string? DefaultLintSuppression => null;

        protected virtual void WriteHeader(FileTemplate fileTemplate, IOutputCache output, bool appendBlankLine = true)
        {
            bool written = false;
            if (fileTemplate.Header?.Description != null)
            {
                output.Add(fileTemplate.Header);
                written = true;
            }
            string? lintSuppression = this.GetLintSuppression(fileTemplate);
            if (lintSuppression != null)
            {
                output.Add(lintSuppression).BreakLine();
                written = true;
            }
            if (written)
            {
                output.If(appendBlankLine).BreakLine().EndIf();
            }
        }

        protected string? GetLintSuppression(FileTemplate fileTemplate)
        {
            if (!fileTemplate.SuppressLint)
            {
                return null;
            }
            string? comment = fileTemplate.Options.GetLintSuppression(fileTemplate.Options.Language?.Name) ?? this.DefaultLintSuppression;
            return string.IsNullOrEmpty(comment) ? null : comment;
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
