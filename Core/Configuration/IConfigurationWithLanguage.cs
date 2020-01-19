using KY.Generator.Languages;

namespace KY.Generator.Configuration
{
    public interface IConfigurationWithLanguage : IConfiguration
    {
        ILanguage Language { get; set; }
        string LanguageKey { get; set; }
    }
}