using System.Collections.Generic;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.TypeScript.Writers
{
    public class TypeScriptFileWriter : FileWriter
    {
        protected override string DefaultLintSuppression => "/* eslint-disable */";

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByTypeAndPath();
        }
    }
}
