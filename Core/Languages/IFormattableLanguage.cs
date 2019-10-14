namespace KY.Generator.Languages
{
    public interface IFormattableLanguage : ILanguage
    {
        LanguageFormatting Formatting { get; }

        string FormatFileName(string fileName, bool isInterface);
        string FormatClassName(string className);
        string FormatPropertyName(string propertyName);
        string FormatFieldName(string fieldName);
        string FormatMethodName(string methodName);
        string FormatParameterName(string parameterName);
    }
}