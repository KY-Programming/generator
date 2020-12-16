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
        public static string FormatFile(string name, IConfiguration configuration, bool force = false)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames && !force)
            {
                return name;
            }
            ConfigurationFormatting formatting = GetFormatting(configuration);
            return configuration.Language is BaseLanguage baseLanguage ? baseLanguage.FormatFileName(name) : Format(name, formatting.FileCase, formatting.AllowedSpecialCharacters);
        }

        public static string FormatClass(string name, IConfiguration configuration, bool force = false)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames && !force)
            {
                return name;
            }
            ConfigurationFormatting formatting = GetFormatting(configuration);
            return Format(name, formatting.ClassCase, formatting.AllowedSpecialCharacters);
        }

        public static string FormatField(string name, IConfiguration configuration, bool force = false)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames && !force)
            {
                return name;
            }
            ConfigurationFormatting formatting = GetFormatting(configuration);
            return Format(name, formatting.FieldCase, formatting.AllowedSpecialCharacters);
        }

        public static string FormatField(string name, ILanguage language, bool formatNames, string allowedCharacters = "")
        {
            return formatNames && language is IFormattableLanguage formattableLanguage ? Format(name, formattableLanguage.Formatting.FieldCase, allowedCharacters) : name;
        }

        public static string FormatProperty(string name, IConfiguration configuration, bool force = false)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames && !force)
            {
                return name;
            }
            ConfigurationFormatting formatting = GetFormatting(configuration);
            return Format(name, formatting.PropertyCase, formatting.AllowedSpecialCharacters);
        }

        public static string FormatMethod(string name, IConfiguration configuration, bool force = false)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames && !force)
            {
                return name;
            }
            ConfigurationFormatting formatting = GetFormatting(configuration);
            return Format(name, formatting.MethodCase, formatting.AllowedSpecialCharacters);
        }

        public static string FormatMethod(string name, ILanguage language, bool formatNames, string allowedCharacters = "")
        {
            return formatNames && language is IFormattableLanguage formattableLanguage ? Format(name, formattableLanguage.Formatting.MethodCase, allowedCharacters) : name;
        }

        public static string FormatParameter(string name, IConfiguration configuration, bool force = false)
        {
            if (configuration is IFormattableConfiguration formattableConfiguration && !formattableConfiguration.FormatNames && !force)
            {
                return name;
            }
            ConfigurationFormatting formatting = GetFormatting(configuration);
            return Format(name, formatting.ParameterCase, formatting.AllowedSpecialCharacters);
        }

        public static string Format(string name, string casing, string allowChars)
        {
            casing.AssertIsNotNullOrEmpty(nameof(casing));
            if (casing.Equals(Case.CamelCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToCamelCase(allowChars);
            }
            if (casing.Equals(Case.PascalCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToPascalCase(allowChars);
            }
            if (casing.Equals(Case.KebabCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToKebabCase(allowChars);
            }
            if (casing.Equals(Case.SnakeCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToSnakeCase(allowChars);
            }
            if (casing.Equals(Case.DarwinCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToDarwinCase(allowChars);
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