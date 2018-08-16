using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Meta;
using KY.Core.Meta.Extensions;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Languages
{
    class StaticFileLanguage : ILanguage, ITemplateWriter
    {
        public string Name => "StaticFileLanguage";

        public virtual void Write(FileTemplate fileTemplate, IOutput output)
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
            IMetaElementList elements = new MetaElementList();
            StaticFileTemplate staticFile = fileTemplate as StaticFileTemplate;
            if (staticFile == null)
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {fileTemplate.GetType().Name} is not implemented in {this.Name}.");
            }
            else
            {
                string fileName = FileSystem.Combine(fileTemplate.RelativePath, fileTemplate.Name);
                this.Write(elements, fileTemplate.Header);
                elements.AddBlankLine();
                FileWriter fileWriter = new FileWriter(output, fileName);
                MetaGenerator metaGenerator = new MetaGenerator(fileWriter, new MetaFormatting());
                metaGenerator.Write(elements);
                fileWriter.Append(staticFile.Content);
                fileWriter.WriteFile();
            }
        }

        public void Write(IMetaFragmentList fragments, ICodeFragment code)
        {
            throw new NotImplementedException($"The method {nameof(Write)} for type {code.GetType().Name} is not implemented in {this.Name}.");
        }

        public void Write(IMetaElementList elements, ICodeFragment code)
        {
            if (code is CommentTemplate)
            {
                new CommentWriter().Write(elements, code);
            }
            else
            {
                throw new NotImplementedException($"The method {nameof(Write)} for type {code.GetType().Name} is not implemented in {this.Name}.");
            }
        }

        public void Write<T>(IMetaFragmentList fragments, IEnumerable<T> code) where T : ICodeFragment
        {
            code.ForEach(fragment => this.Write(fragments, fragment));
        }

        public void Write<T>(IMetaElementList elements, IEnumerable<T> code) where T : ICodeFragment
        {
            code.ForEach(fragment => this.Write(elements, fragment));
        }
    }
}
