using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptFileWriter : FileWriter
    {
        public TypeScriptFileWriter(IOptions options)
            : base(options)
        { }

        protected override void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            fileTemplate.Header.Description += Environment.NewLine + "/* eslint-disable */" + Environment.NewLine + "tslint:disable";
            base.WriteHeader(fileTemplate, output);
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByTypeAndPath();
        }
    }
}
