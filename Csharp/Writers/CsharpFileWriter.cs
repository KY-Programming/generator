using System;
using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class CsharpFileWriter : FileWriter
    {
        public CsharpFileWriter(IOptions options)
            : base(options)
        { }

        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            FileTemplate template = (FileTemplate)fragment;
            INamespaceChildren children = template.Namespaces.FirstOrDefault()?.Children.FirstOrDefault();
            UsingTemplate usingTemplate = new("System.CodeDom.Compiler", null, null);
            children?.Usings.Add(usingTemplate);
            base.Write(template, output);
            children?.Usings.Remove(usingTemplate);
        }

        protected override void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            fileTemplate.Header.Description += Environment.NewLine + "ReSharper disable All";
            base.WriteHeader(fileTemplate, output);
        }
    }
}
