namespace KY.Generator.Languages
{
    public class EmptyFormattableLanguage : EmptyLanguage, IFormattableLanguage
    {
        public override string Name => "EmptyFormattable";
        public LanguageFormatting Formatting { get; } = new();

        public string FormatFileName(string fileName, string fileType = null)
        {
            return fileName;
        }
    }
}
