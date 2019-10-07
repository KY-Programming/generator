using KY.Generator.Languages;

namespace KY.Generator.Csharp.Languages
{
    public static class LanguageExtension
    {
        public static bool IsCsharp(this ILanguage language)
        {
            return language is CsharpLanguage;
        }
    }
}