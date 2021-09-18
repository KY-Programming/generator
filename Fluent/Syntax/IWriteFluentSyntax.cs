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
        /// Modify the file name
        /// </summary>
        IWriteFluentSyntax FileName(Action<IFileNameFluentSyntax> action);
    }
}
