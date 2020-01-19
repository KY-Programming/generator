using KY.Generator.Languages;

namespace KY.Generator.OpenApi.Languages
{
    class OpenApiLanguage : EmptyLanguage
    {
        public static OpenApiLanguage Instance { get; } = new OpenApiLanguage();

        public override string Name => "OpenApi";

        private OpenApiLanguage()
        { }
    }
}
