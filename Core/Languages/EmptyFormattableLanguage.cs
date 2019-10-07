namespace KY.Generator.Languages
{
    public class EmptyFormattableLanguage : EmptyLanguage, IFormattableLanguage
    {
        public override string Name => "EmptyFormattable";
        public LanguageFormatting Formatting { get; } = new LanguageFormatting();

        public string FormatFileName(string fileName, bool isInterface)
        {
            return fileName;
        }

        public string FormatClassName(string className)
        {
            return className;
        }

        public string FormatPropertyName(string propertyName)
        {
            return propertyName;
        }

        public string FormatFieldName(string fieldName)
        {
            return fieldName;
        }

        public string FormatMethodName(string methodName)
        {
            return methodName;
        }
    }
}