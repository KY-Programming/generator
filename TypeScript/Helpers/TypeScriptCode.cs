using System.Text.RegularExpressions;
using KY.Core;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript
{
    public static class TypeScriptCode
    {
        public static TypeScriptTemplate TypeScript(this Code _, string code)
        {
            return new TypeScriptTemplate(code);
        }

        public static string GetFileName(this Code _, string fileName, bool isInterface = false)
        {
            //Ignore all files with all uppercase like TEST_FILE
            if (fileName == fileName.ToUpperInvariant())
            {
                return fileName + ".ts";
            }
            fileName = Regex.Replace(fileName, "[A-Z]", x => "-" + x.Value.ToLowerInvariant()).Trim('-');
            if (fileName.StartsWith("i-") || isInterface)
            {
                fileName = fileName.TrimStart("i-") + ".interface";
            }
            return fileName + ".ts";
        }
    }
}