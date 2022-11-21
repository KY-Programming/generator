using System;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Models;

namespace KY.Generator
{
    public static class Formatter
    {
        public static string FormatFile(string name, IOptions options, string type = null, bool force = true)
        {
            return options.Language?.FormatFile(name, options, type, force) ?? Format(name, options.Formatting.FileCase, options, force);
        }

        public static string FormatClass(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.ClassCase, options, force);
        }

        public static string FormatField(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.FieldCase, options, force, options.Formatting.CaseMode);
        }

        public static string FormatProperty(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.PropertyCase, options, force, options.Formatting.CaseMode);
        }

        public static string FormatMethod(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.MethodCase, options, force);
        }

        public static string FormatParameter(string name, IOptions options, bool force = false)
        {
            return Format(name, options.Formatting.ParameterCase, options, force);
        }

        public static string Format(string name, string casing, IOptions options, bool force = false, CaseMode mode = CaseMode.Fix)
        {
            if (options.FormatNames || force)
            {
                return Format(name, casing, options, mode);
            }
            return name;
        }

        public static string Format(string name, string casing, IOptions options, CaseMode mode)
        {
            casing.AssertIsNotNullOrEmpty(nameof(casing));
            if (options.Language.ReservedKeywords.ContainsKey(name))
            {
                name = options.Language.ReservedKeywords[name];
            }
            if (mode == CaseMode.AspDotNetCompatible)
            {
                return name?.ToAspDotNetCompatibleFirstLowerCase();
            }
            if (casing.Equals(Case.CamelCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToCamelCase(options.Formatting.AllowedSpecialCharacters);
            }
            if (casing.Equals(Case.PascalCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToPascalCase(options.Formatting.AllowedSpecialCharacters);
            }
            if (casing.Equals(Case.KebabCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToKebabCase(options.Formatting.AllowedSpecialCharacters);
            }
            if (casing.Equals(Case.SnakeCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToSnakeCase(options.Formatting.AllowedSpecialCharacters);
            }
            if (casing.Equals(Case.DarwinCase, StringComparison.CurrentCultureIgnoreCase))
            {
                return name?.ToDarwinCase(options.Formatting.AllowedSpecialCharacters);
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
