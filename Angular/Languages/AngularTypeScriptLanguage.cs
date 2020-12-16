using System;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Angular.Languages
{
    internal class AngularTypeScriptLanguage : TypeScriptLanguage
    {
        public AngularTypeScriptLanguage()
        {
            this.Key = TypeScriptLanguage.Instance.Key;
        }

        public override string FormatFileName(string fileName, string fileType = null)
        {
            string formattedFileName = base.FormatFileName(fileName, fileType);
            if ("service".Equals(fileType, StringComparison.CurrentCultureIgnoreCase))
            {
                formattedFileName = formattedFileName.Replace("-service.ts", ".service.ts");
            }
            return formattedFileName;
        }
    }
}