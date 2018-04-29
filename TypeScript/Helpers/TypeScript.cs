using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator
{
    public static class TypeScript
    {
        public static TypeScriptLanguage Language { get; } = new TypeScriptLanguage();

        public static TypeScriptTemplate Code(string code)
        {
            return new TypeScriptTemplate(code);
        }
    }
}
