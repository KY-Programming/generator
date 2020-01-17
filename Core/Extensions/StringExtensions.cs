using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KY.Core;

namespace KY.Generator.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to PascalCase (UpperCamelCase)
        /// </summary>
        public static string ToPascalCase(this string value)
        {
            return string.Join("", Split(value, true).Select(x => x.FirstCharToUpper()));
        }
        
        /// <summary>
        /// Converts a string to camelCase (lowerCamelCase)
        /// </summary>
        public static string ToCamelCase(this string value)
        {
            return string.Join("", Split(value, true).Select(x => x.FirstCharToUpper())).FirstCharToLower();
        }
        
        /// <summary>
        /// Converts a string to kebab-case
        /// </summary>
        public static string ToKebabCase(this string value)
        {
            return string.Join("-", Split(value, true).Select(x => x.ToLowerInvariant()));
        }
        
        /// <summary>
        /// Converts a string to snake_case
        /// </summary>
        public static string ToSnakeCase(this string value)
        {
            return string.Join("_", Split(value, true).Select(x => x.ToLowerInvariant()));
        }
        
        /// <summary>
        /// Converts a string to Darwin_Case
        /// </summary>
        public static string ToDarwinCase(this string value)
        {
            return string.Join("_", Split(value, true).Select(x => x.ToLowerInvariant().FirstCharToUpper()));
        }

        private static IEnumerable<string> Split(string value, bool skipSpecialChars)
        {
            if (value.ToUpperInvariant() == value)
            {
                value = value.ToLowerInvariant();
            }
            List<string> matches = Regex.Matches(value, @"([a-z]+|[A-Z]+|[0-9]+|[^a-zA-Z0-9]+)").Cast<Match>().Select(x => x.Value)
                                        .Where(x => !skipSpecialChars || Regex.IsMatch(x, @"^[a-zA-Z0-9]+$"))
                                        .ToList();
            string transfer = string.Empty;
            for (int index = 0; index < matches.Count; index++)
            {
                string match = transfer + matches[index];
                transfer = string.Empty;
                bool allUpper = match.ToUpperInvariant() == match;
                bool nextAllLower = matches.Count > index + 1 && matches[index + 1].ToLowerInvariant() == matches[index + 1];
                if (allUpper && nextAllLower)
                {
                    if (match.Length == 1)
                    {
                        match = match + matches[index + 1];
                        index++;
                    }
                    else
                    {
                        transfer = match.Substring(match.Length - 1, 1);
                        match = match.Substring(0, match.Length - 1);
                    }
                }
                yield return match;
            }
        }

        private static CaseType GetCaseType(char value)
        {
            string input = value.ToString();
            if (Regex.IsMatch(input, "[a-z]"))
            {
                return CaseType.Lower;
            }
            if (Regex.IsMatch(input, "[A-Z]"))
            {
                return CaseType.Upper;
            }
            if (Regex.IsMatch(input, "[0-9]"))
            {
                return CaseType.Number;
            }
            return CaseType.Special;
        }

        private enum CaseType
        {
            Lower,
            Upper,
            Number,
            Special
        }
    }
}