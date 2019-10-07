using KY.Generator.Languages;

namespace KY.Generator.Configurations
{
    public interface IFormattableConfiguration
    {
        bool FieldsToProperties { get; }
        bool PropertiesToFields { get; }
        bool FormatNames { get; }
        ILanguage Language { get; }
    }
}