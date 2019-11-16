namespace KY.Generator.Languages
{
    public interface IFormattableLanguage : ILanguage
    {
        LanguageFormatting Formatting { get; }
    }
}