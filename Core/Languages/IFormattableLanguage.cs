namespace KY.Generator.Languages
{
    public interface IFormattableLanguage : ILanguage
    {
        LanguageFormatting Formatting { get; }

        string FormatFileName(string fileName, string fileType = null);
    }
}
