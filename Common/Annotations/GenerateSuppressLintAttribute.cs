namespace KY.Generator;

/// <summary>
/// Replaces the comment that switches the linter off for a generated file. Each language brings its own comment
/// (TypeScript <code>/* eslint-disable */</code>, C# <code>// ReSharper disable All</code>), so the comment is only
/// needed for a different linter. An empty comment writes none at all.
/// Without a <see cref="Language"/> the comment is used for every language the element is generated to. Set the
/// language - e.g. <c>nameof(OutputLanguage.TypeScript)</c> - to replace the comment of that one language only and
/// leave the others with the comment of their language.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public class GenerateSuppressLintAttribute : Attribute
{
    public string Comment { get; }

    /// <summary>
    /// The language the comment is written for, <c>null</c> for every language
    /// </summary>
    public string? Language { get; }

    public GenerateSuppressLintAttribute(string comment)
        : this(comment, null)
    { }

    public GenerateSuppressLintAttribute(string comment, string? language)
    {
        this.Comment = comment;
        this.Language = language;
    }
}
