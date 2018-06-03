using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript
{
    public static class Code
    {
        public static TypeScriptLanguage Language { get; } = new TypeScriptLanguage();

        public static TypeScriptTemplate TypeScript(string code)
        {
            return new TypeScriptTemplate(code);
        }
    }
}