using System.Text.RegularExpressions;

namespace KY.Generator.AspDotNet;

/// <summary>
/// Compares api versions the way Asp.Versioning does: by group version, major, minor and status, where a version
/// without status (1.0) is newer than the same version with one (1.0-beta). Versions that can not be parsed are
/// compared as plain text, so the order stays deterministic for any input.
/// </summary>
public class ApiVersionComparer : IComparer<string>
{
    private static readonly Regex pattern = new(@"^\s*((?<group>\d{4}-\d{2}-\d{2})\.?)?((?<major>\d+)(\.(?<minor>\d+))?)?(-(?<status>\w+))?\s*$", RegexOptions.Compiled);

    public static ApiVersionComparer Instance { get; } = new();

    public int Compare(string? left, string? right)
    {
        if (left == right)
        {
            return 0;
        }
        if (left == null)
        {
            return -1;
        }
        if (right == null)
        {
            return 1;
        }
        Match leftMatch = pattern.Match(left);
        Match rightMatch = pattern.Match(right);
        if (!leftMatch.Success || !rightMatch.Success)
        {
            return string.Compare(left, right, StringComparison.OrdinalIgnoreCase);
        }
        int result = string.Compare(GetText(leftMatch, "group"), GetText(rightMatch, "group"), StringComparison.OrdinalIgnoreCase);
        result = result != 0 ? result : GetNumber(leftMatch, "major").CompareTo(GetNumber(rightMatch, "major"));
        result = result != 0 ? result : GetNumber(leftMatch, "minor").CompareTo(GetNumber(rightMatch, "minor"));
        return result != 0 ? result : CompareStatus(GetText(leftMatch, "status"), GetText(rightMatch, "status"));
    }

    private static int CompareStatus(string leftStatus, string rightStatus)
    {
        if (leftStatus.Length == 0 || rightStatus.Length == 0)
        {
            // A version without status is the released one and therefore the newer of the two
            return rightStatus.Length - leftStatus.Length;
        }
        return string.Compare(leftStatus, rightStatus, StringComparison.OrdinalIgnoreCase);
    }

    private static string GetText(Match match, string group)
    {
        return match.Groups[group].Success ? match.Groups[group].Value : string.Empty;
    }

    private static int GetNumber(Match match, string group)
    {
        return match.Groups[group].Success ? int.Parse(match.Groups[group].Value) : 0;
    }
}
