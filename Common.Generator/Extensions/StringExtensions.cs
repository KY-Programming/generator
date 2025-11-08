using System.Text.RegularExpressions;
using KY.Core.Extension;

namespace KY.Generator.Extensions;

public static class StringExtensions
{
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
        CaseType firstCharCase = value[0].GetCaseType();
        CaseType secondCharCase = value[1].GetCaseType();
        CaseType prefixCase = prefix[0].GetCaseType();
        if (firstCharCase != prefixCase || firstCharCase != secondCharCase)
        {
            return prefix + value;
        }
        return value;
    }

    public static string Replace(this string value, IReadOnlyDictionary<string, string>? replaceName)
    {
        return replaceName == null ? value : replaceName.Aggregate(value, (current, pair) => current.Replace(pair.Key, pair.Value));
    }
}
