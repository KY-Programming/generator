namespace KY.Generator.Languages
{
    public class EmptyFormattableLanguage : EmptyLanguage, IFormattableLanguage
    {
        public override string Name => "EmptyFormattable";
        public LanguageFormatting Formatting { get; } = new LanguageFormatting();
    }
}
