using System.Collections.Generic;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptFileWriter : FileWriter
    {
        protected override void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            base.WriteHeader(fileTemplate, output);
            Dictionary<string, bool> linters = fileTemplate.Linters ?? new Dictionary<string, bool> { { "eslint", false }, { "tslint", false } };
            foreach (KeyValuePair<string, bool> linter in linters)
            {
                switch (linter.Key.ToLower())
                {
                    case "eslint":
                        output.Add("/* eslint-disable */");
                        break;
                    case "tslint":
                        output.Add("// tslint:disable");
                        break;
                }
                output.BreakLine();
            }
            output.BreakLine();
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByTypeAndPath();
        }
    }
}
