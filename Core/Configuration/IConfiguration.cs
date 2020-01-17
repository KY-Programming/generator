using KY.Generator.Configuration;
using KY.Generator.Languages;
using KY.Generator.Output;

namespace KY.Generator.Configurations
{
    public interface IConfiguration
    {
        ILanguage Language { get; }
        ConfigurationFormatting Formatting { get; }
        IOutput Output { get; }
        ConfigurationEnvironment Environment { get; }
    }
}