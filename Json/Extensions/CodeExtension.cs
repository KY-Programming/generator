using KY.Generator.Json.Language;
using KY.Generator.Languages;

namespace KY.Generator.Json.Extensions
{
    public static class CodeExtension
    {
        private static readonly JsonLanguage language = new JsonLanguage();

        public static JsonLanguage Json(this ILanguageList list)
        {
            return language;
        }
    }
}