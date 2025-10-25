using KY.Generator.Languages;

namespace KY.Generator.Json.Language
{
    public class JsonLanguage : EmptyLanguage
    {
        public static JsonLanguage Instance { get; } = new JsonLanguage();

        public override string Name => "Json";

        private JsonLanguage()
        { }
    }
}