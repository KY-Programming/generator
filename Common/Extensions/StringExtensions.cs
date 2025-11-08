using System.Text.RegularExpressions;

namespace KY.Generator.Extensions;

// Clone from KY.Core
internal static class CoreStringExtensions
{
    public static string FirstCharToLower(this string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value[0].ToString().ToLower() + value.Substring(1);
        }
        return value;
    }

    public static string FirstCharToUpper(this string value)
    {
        return string.IsNullOrEmpty(value) ? value : value[0].ToString().ToUpper() + value.Substring(1);
    }
}

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

    public static string ToAspDotNetCompatibleFirstLowerCase(this string value)
    {
        Match match = new Regex(@"^(?<upper>[A-Z]*)(?<special>[A-Z])(?<else>.*)$").Match(value);
        if (!match.Success)
        {
            return value;
        }
        if (match.Groups["else"].Length == 0)
        {
            return value.ToLowerInvariant();
        }
        if (match.Groups["upper"].Length == 0)
        {
            return match.Groups["special"].Value.ToLowerInvariant() + match.Groups["else"].Value;
        }
        return match.Groups["upper"].Value.ToLowerInvariant() + match.Groups["special"].Value + match.Groups["else"].Value;
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
}
