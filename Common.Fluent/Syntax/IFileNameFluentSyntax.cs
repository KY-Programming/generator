namespace KY.Generator;

public interface IFileNameFluentSyntax
{
    /// <summary>
    /// A regex replace like <see cref="System.Text.RegularExpressions.Regex.Replace(string,string)"/>
    /// </summary>
    /// <param name="pattern">A regex pattern e.g. "^my-(.*)$"</param>
    /// <param name="replacement">A regex replacement e.g. "$1..."</param>
    /// <param name="matchingType">The required type of the file. Default is null. Null means all</param>
    IFileNameFluentSyntax Replace(string pattern, string replacement, string matchingType = null);
}
