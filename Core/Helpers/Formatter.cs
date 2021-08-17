using System;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Languages;

namespace KY.Generator
{
    public static class Formatter
    {
        public static string FormatFile(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.FileCase, options, force);
        }

        public static string FormatClass(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.ClassCase, options, force);
        }

        public static string FormatField(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.FieldCase, options, force);
        }

        public static string FormatProperty(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.PropertyCase, options, force);
        }

        public static string FormatMethod(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.MethodCase, options, force);
        }

        public static string FormatParameter(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.ParameterCase, options, force);
        }

        public static string Format(string name, string casing, IOptions options, bool force)
        {
            if (options.FormatNames || force)
            {
                return Format(name, casing, options.Formatting.AllowedSpecialCharacters);
            }
            return name;
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
    }
}
