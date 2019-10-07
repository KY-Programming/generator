using KY.Generator.Languages;

namespace KY.Generator.TypeScript.Languages
{
    public static class LanguageExtension
    {
        public static bool IsTypeScript(this ILanguage language)
        {
            return language is TypeScriptLanguage;
        }
    }
}