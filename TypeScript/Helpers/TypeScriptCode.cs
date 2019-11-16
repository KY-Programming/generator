using KY.Generator.TypeScript.Templates;

namespace KY.Generator.TypeScript
{
    public static class TypeScriptCode
    {
        public static TypeScriptTemplate TypeScript(this Code _, string code)
        {
            return new TypeScriptTemplate(code);
        }
    }
}