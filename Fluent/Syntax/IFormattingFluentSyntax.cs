namespace KY.Generator.Syntax
{
    public interface IFormattingFluentSyntax
    {
        /// <summary>
        /// Case of a file. <see cref="KY.Generator.Case"/>
        /// </summary>
        /// <param name="casing"></param>
        /// <returns></returns>
        IFormattingFluentSyntax FileCase(string casing);

        /// <summary>
        /// Case of a class. <see cref="KY.Generator.Case"/>
        /// </summary>
        /// <param name="casing"></param>
        /// <returns></returns>
        IFormattingFluentSyntax ClassCase(string casing);

        /// <summary>
        /// Case of a field. <see cref="KY.Generator.Case"/>
        /// </summary>
        /// <param name="casing"></param>
        /// <returns></returns>
        IFormattingFluentSyntax FieldCase(string casing);

        /// <summary>
        /// Case of a property. <see cref="KY.Generator.Case"/>
        /// </summary>
        /// <param name="casing"></param>
        /// <returns></returns>
        IFormattingFluentSyntax PropertyCase(string casing);

        /// <summary>
        /// Case of a method. <see cref="KY.Generator.Case"/>
        /// </summary>
        /// <param name="casing"></param>
        /// <returns></returns>
        IFormattingFluentSyntax MethodCase(string casing);

        /// <summary>
        /// Case of a parameter. <see cref="KY.Generator.Case"/>
        /// </summary>
        /// <param name="casing"></param>
        /// <returns></returns>
        IFormattingFluentSyntax ParameterCase(string casing);

        /// <summary>
        /// Define with characters are allowed in class, property, field or parameter names. Default is none
        /// </summary>
        /// <param name="specialCharacters"></param>
        /// <returns></returns>
        IFormattingFluentSyntax AllowedSpecialCharacters(string specialCharacters);

        /// <summary>
        /// Forces the generator to use spaces for indenting
        /// </summary>
        /// <param name="spaces"></param>
        /// <returns></returns>
        IFormattingFluentSyntax UseWhitespaces(int spaces = 4);

        /// <summary>
        /// Forces the generator to use tabs for indenting
        /// </summary>
        /// <param name="tabs"></param>
        /// <returns></returns>
        IFormattingFluentSyntax UseTab(int tabs = 1);

        /// <summary>
        /// Defines a char for quotes. E.g single ' or double "
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        IFormattingFluentSyntax Quotes(string quote);

        /// <summary>
        /// Forces the brace of a block to stay in same line. E.g. <code>if (...) {...</code> or <code>class Dummy {...</code>
        /// </summary>
        /// <returns></returns>
        IFormattingFluentSyntax NoStartBlockInNewLine();

        /// <summary>
        /// Per default all files ends with a empty line
        /// </summary>
        /// <returns></returns>
        IFormattingFluentSyntax NoEndFileWithNewLine();

        /// <summary>
        /// Collapses a class if no properties or values exists. E.g. <code>class Dummy { }</code>
        /// </summary>
        /// <returns></returns>
        IFormattingFluentSyntax CollapseEmptyClasses();

        /// <summary>
        /// The string between the braces of a empty class. E.g. space, empty string or tab
        /// </summary>
        /// <param name="spacer"></param>
        /// <returns></returns>
        IFormattingFluentSyntax CollapsedClassesSpacer(string spacer);
    }
}
