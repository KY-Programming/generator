namespace KY.Generator.Languages.Extensions
{
    public static class FormattingOptionsExtension
    {
        public static void FromLanguage(this FormattingOptions options, ILanguage language)
        {
            if (language is not IFormattableLanguage formattableLanguage)
            {
                return;
            }
            options.ClassCase = formattableLanguage.Formatting.ClassCase;
            options.FieldCase = formattableLanguage.Formatting.FieldCase;
            options.FileCase = formattableLanguage.Formatting.FileCase;
            options.MethodCase = formattableLanguage.Formatting.MethodCase;
            options.ParameterCase = formattableLanguage.Formatting.ParameterCase;
            options.PropertyCase = formattableLanguage.Formatting.PropertyCase;
            options.AllowedSpecialCharacters = formattableLanguage.Formatting.AllowedSpecialCharacters;
        }
    }
}
