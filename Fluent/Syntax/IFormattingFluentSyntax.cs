namespace KY.Generator.Syntax
{
    public interface IFormattingFluentSyntax
    {
        IFormattingFluentSyntax UseWhitespaces(int spaces = 4);
        IFormattingFluentSyntax UseTab(int tabs = 1);
        IFormattingFluentSyntax Quotes(string quote);
    }
}
