using KY.Generator.Languages;

namespace KY.Generator.Templates.Extensions
{
    public static class ParameterTemplateExtension
    {
        public static ParameterTemplate FormatName(this ParameterTemplate parameter, ILanguage language, bool formatNames = true)
        {
            if (formatNames && language is IFormattableLanguage formattableLanguage)
            {
                parameter.Name = formattableLanguage.FormatParameterName(parameter.Name);
            }
            return parameter;
        }
    }
}