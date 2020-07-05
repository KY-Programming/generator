using System;
using KY.Core;
using KY.Generator.Configuration;
using KY.Generator.Configurations;
using KY.Generator.Extensions;
using KY.Generator.Languages;

namespace KY.Generator
{
    public static class Formatter
    {
        public static string FormatFile(string name, IConfiguration configuration)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames)
            {
                return name;
            }
            return configuration.Language is BaseLanguage baseLanguage ? baseLanguage.FormatFileName(name, false) : Format(name, GetFormatting(configuration).FileCase);
        }

        public static string FormatClass(string name, IConfiguration configuration)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames)
            {
                return name;
            }
            return Format(name, GetFormatting(configuration).ClassCase);
        }

        public static string FormatField(string name, IConfiguration configuration)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames)
            {
                return name;
            }
            return Format(name, GetFormatting(configuration).FieldCase);
        }

        public static string FormatField(string name, ILanguage language, bool formatNames)
        {
            return formatNames && language is IFormattableLanguage formattableLanguage ? Format(name, formattableLanguage.Formatting.FieldCase) : name;
        }

        public static string FormatProperty(string name, IConfiguration configuration)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames)
            {
                return name;
            }
            return Format(name, GetFormatting(configuration).PropertyCase);
        }

        public static string FormatMethod(string name, IConfiguration configuration)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames)
            {
                return name;
            }
            return Format(name, GetFormatting(configuration).MethodCase);
        }

        public static string FormatMethod(string name, ILanguage language, bool formatNames)
        {
            return formatNames && language is IFormattableLanguage formattableLanguage ? Format(name, formattableLanguage.Formatting.MethodCase) : name;
        }

        public static string FormatParameter(string name, IConfiguration configuration)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames)
            {
                return name;
            }
            return Format(name, GetFormatting(configuration).ParameterCase);
        }

        public static string Format(string name, string casing)
        {
            casing.AssertIsNotNullOrEmpty(nameof(casing));
            if (casing.Equals(Case.CamelCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToCamelCase();
            }
            if (casing.Equals(Case.PascalCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToPascalCase();
            }
            if (casing.Equals(Case.KebabCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToKebabCase();
            }
            if (casing.Equals(Case.SnakeCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToSnakeCase();
            }
            if (casing.Equals(Case.DarwinCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToDarwinCase();
            }
            if (casing.Equals(Case.FirstCharToLower, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.FirstCharToLower();
            }
            if (casing.Equals(Case.FirstCharToUpper, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.FirstCharToUpper();
            }
            throw new ArgumentOutOfRangeException(nameof(casing));
        }

        private static ConfigurationFormatting GetFormatting(IConfiguration configuration)
        {
            ConfigurationFormatting formatting = configuration.Formatting;
            if (configuration.Language is IFormattableLanguage formattableLanguage)
            {
                if (formatting == null)
                {
                    formatting = formattableLanguage.Formatting;
                }
                else
                {
                    formatting.ApplyDefaults(formattableLanguage.Formatting);
                }
            }
            return formatting;
        }
    }
}