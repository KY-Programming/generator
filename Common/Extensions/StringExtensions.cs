using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using KY.Core;

namespace KY.Generator.Extensions
{
    public static class StringExtensions
    {
        // TODO: Move to KY.Core.Common
        public static string PadLeft(this string value, int totalWidth, string text = " ", bool exact = false)
        {
            if (value == null || text == null || value.Length >= totalWidth)
            {
                return value;
            }
            StringBuilder builder = new();
            while (builder.Length + value.Length < totalWidth)
            {
                builder.Append(text);
            }
            builder.Append(value);
            return exact ? builder.ToString().Substring(0, totalWidth) : builder.ToString();
        }

        // TODO: Move to KY.Core.Common
        public static string PadRight(this string value, int totalWidth, string text = " ", bool exact = false)
        {
            if (value == null || text == null || value.Length >= totalWidth)
            {
                return value;
            }
            StringBuilder builder = new();
            builder.Append(value);
            while (builder.Length < totalWidth)
            {
                builder.Append(text);
            }
            return exact ? builder.ToString().Substring(0, totalWidth) : builder.ToString();
        }

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

        public static int IndexOf(this string value, Regex regex)
        {
            return regex.Match(value).Index;
        }

        public static string Prefix(this string value, string prefix)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(prefix))
            {
                return value;
            }
            if (!value.StartsWith(prefix))
            {
                return prefix + value;
            }
            CaseType firstCharCase = GetCaseType(value[0]);
            CaseType secondCharCase = GetCaseType(value[1]);
            CaseType prefixCase = GetCaseType(prefix[0]);
            if (firstCharCase != prefixCase || firstCharCase != secondCharCase)
            {
                return prefix + value;
            }
            return value;
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
