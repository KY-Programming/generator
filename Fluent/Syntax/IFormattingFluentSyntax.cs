using KY.Generator.Models;

namespace KY.Generator.Syntax
{
    public interface IFormattingFluentSyntax
    {
        /// <summary>
        /// Case of a file. <see cref="KY.Generator.Case"/>
        /// </summary>
        IFormattingFluentSyntax FileCase(string casing);

        /// <summary>
        /// Case of a class. <see cref="KY.Generator.Case"/>
        /// </summary>
        IFormattingFluentSyntax ClassCase(string casing);

        /// <summary>
        /// Case of a field. <see cref="KY.Generator.Case"/>
        /// </summary>
        IFormattingFluentSyntax FieldCase(string casing);

        /// <summary>
        /// Case of a property. <see cref="KY.Generator.Case"/>
        /// </summary>
        IFormattingFluentSyntax PropertyCase(string casing);

        /// <summary>
        /// Case of a method. <see cref="KY.Generator.Case"/>
        /// </summary>
        IFormattingFluentSyntax MethodCase(string casing);

        /// <summary>
        /// Case of a parameter. <see cref="KY.Generator.Case"/>
        /// </summary>
        IFormattingFluentSyntax ParameterCase(string casing);

        /// <summary>
        /// Compatibility mode for casing e.g. ASP.NET serializer converts MY_TEST to mY_TEST instead of expected myTest
        /// </summary>
        IFormattingFluentSyntax CaseMode(CaseMode mode);

        /// <summary>
        /// Define with characters are allowed in class, property, field or parameter names. Default is none
        /// </summary>
        IFormattingFluentSyntax AllowedSpecialCharacters(string specialCharacters);

        /// <summary>
        /// Forces the generator to use spaces for indenting
        /// </summary>
        IFormattingFluentSyntax UseWhitespaces(int spaces = 4);

        /// <summary>
        /// Forces the generator to use tabs for indenting
        /// </summary>
        IFormattingFluentSyntax UseTab(int tabs = 1);

        /// <summary>
        /// Defines a char for quotes. E.g single ' or double "
        /// </summary>
        IFormattingFluentSyntax Quotes(string quote);

        /// <summary>
        /// Forces the brace of a block to stay in same line. E.g. <code>if (...) {...</code> or <code>class Dummy {...</code>
        /// </summary>
        IFormattingFluentSyntax NoStartBlockInNewLine();

        /// <summary>
        /// Per default all files ends with a empty line
        /// </summary>
        IFormattingFluentSyntax NoEndFileWithNewLine();

        /// <summary>
        /// Collapses a class if no properties or values exists. E.g. <code>class Dummy { }</code>
        /// </summary>
        /// <param name="spacer">The string between the braces of a empty class. E.g. space, empty string or tab</param>
        IFormattingFluentSyntax CollapseEmptyClasses(string spacer = " ");

        /// <summary>
        /// Prefix to add to class name. E.g if you set "C" as prefix <code>MyClass</code> => <code>CMyClass</code>
        /// </summary>
        IFormattingFluentSyntax ClassPrefix(string prefix);

        /// <summary>
        /// Prefix to add to interface name. E.g if you set "I" as prefix <code>MyInterface</code> => <code>IMyInterface</code>
        /// </summary>
        IFormattingFluentSyntax InterfacePrefix(string prefix);

        IFormattingFluentSyntax AddFileNameReplacer(string key, string pattern, string replacement, string matchingType = null);
        IFormattingFluentSyntax SetFileNameReplacer(string key, string replacement);
        IFormattingFluentSyntax AddOrSetFileNameReplacer(string key, string pattern, string replacement, string matchingType = null);
    }
}
