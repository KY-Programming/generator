using System;

namespace KY.Generator.Syntax
{
    public interface IWriteFluentSyntax
    {
        /// <summary>
        /// Code formatting guidelines
        /// </summary>
        IWriteFluentSyntax Formatting(Action<IFormattingFluentSyntax> action);

        /// <summary>
        /// Forces the generator to skip the default header (NOT RECOMMENDED)
        /// </summary>
        IWriteFluentSyntax NoHeader();

        /// <summary>
        /// Forces the generator to not check if file has changes and always overwrites a file (NOT RECOMMENDED)
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
}
