using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KY.Core;

namespace KY.Generator.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string value, string allowedCharacters = "")
        {
            return string.Join("", Split(value, allowedCharacters).Select(x => x.FirstCharToUpper()));
        }

        public static string ToCamelCase(this string value, string allowedCharacters = "")
        {
            return string.Join("", Split(value, allowedCharacters).Select(x => x.FirstCharToUpper())).FirstCharToLower();
        }

        public static string ToKebabCase(this string value, string allowedCharacters = "")
        {
            return string.Join("-", Split(value, allowedCharacters).Select(x => x.ToLowerInvariant()));
        }

        public static string ToSnakeCase(this string value, string allowedCharacters = "")
        {
            return string.Join("_", Split(value, allowedCharacters).Select(x => x.ToLowerInvariant()));
        }

        public static string ToDarwinCase(this string value, string allowedCharacters = "")
        {
            return string.Join("_", Split(value, allowedCharacters).Select(x => x.ToLowerInvariant().FirstCharToUpper()));
        }

        private static IEnumerable<string> Split(string value, string allowedCharacters)
        {
            if (value.ToUpperInvariant() == value)
            {
                value = value.ToLowerInvariant();
            }
            List<string> matches = Regex.Matches(value, @"([a-z]+|[A-Z]+|[0-9]+|[^a-zA-Z0-9]+)").Cast<Match>().Select(x => x.Value)
                                        .Where(x => Regex.IsMatch(x, $"^[a-zA-Z0-9{Regex.Escape(allowedCharacters ?? string.Empty)}]+$"))
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