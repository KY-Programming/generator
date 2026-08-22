namespace KY.Generator;

public interface IWriteFluentSyntax
{
    /// <summary>
    /// Code formatting guidelines
    /// </summary>
    IWriteFluentSyntax Formatting(Action<IFormattingFluentSyntax> action);

    /// <summary>
    /// Skips the <code>&lt;auto-generated&gt;</code> header. DO NOT USE THIS IN PRODUCTION. This is only meant for unit testing
    /// </summary>
    IWriteFluentSyntax NoHeader();

    /// <summary>
    /// Writes the <code>&lt;auto-generated&gt;</code> header without the version of the generator, so an updated
    /// generator alone does not change every generated file
    /// </summary>
    IWriteFluentSyntax NoHeaderVersion();

    /// <summary>
    /// Replaces the comment that switches the linter off for a generated file. Each language brings its own comment
    /// (TypeScript <code>/* eslint-disable */</code>, C# <code>// ReSharper disable All</code>), so the comment is
    /// only needed for a different linter. An empty comment writes none at all.
    /// Without a language the comment is used for every language of this write. Pass one - e.g.
    /// <c>nameof(OutputLanguage.TypeScript)</c> - to replace the comment of that one language only and leave the
    /// others with the comment of their language
    /// </summary>
    IWriteFluentSyntax SuppressLint(string comment, string? language = null);

    /// <summary>
    /// Forces the generator to not check if a file has changes and always overwrites a file (NOT RECOMMENDED)
    /// </summary>
    IWriteFluentSyntax ForceOverwrite();

    /// <summary>
    /// Modify the file name
    /// </summary>
    IWriteFluentSyntax FileName(Action<IFileNameFluentSyntax> action);

    /// <summary>
    /// Executes a formatter after a file is generated and written to disk.
    /// Available variables:
    /// <list type="bullet">
    ///     <item>
    ///         <term>$file</term>
    ///         <description>The absolute path to the changed file</description>
    ///     </item>
    ///     <item>
    ///         <term>$project</term>
    ///         <description>The absolute path to the project folder (path to the project file, without the file name)</description>
    ///     </item>
    ///     <item>
    ///         <term>$output</term>
    ///         <description>The absolute path to the output folder</description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    IWriteFluentSyntax Formatter(string command);
}
