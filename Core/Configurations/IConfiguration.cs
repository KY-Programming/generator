using KY.Generator.Configuration;
using KY.Generator.Languages;

namespace KY.Generator.Configurations
{
    public interface IConfiguration
    {
        ILanguage Language { get; }
        ConfigurationFormatting Formatting { get; }
    }
}