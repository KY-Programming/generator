namespace KY.Generator.Syntax
{
    public class FormattingFluentSyntax : IFormattingFluentSyntax
    {
        private readonly FormattingOptions options;

        public FormattingFluentSyntax(FormattingOptions options)
        {
            this.options = options;
        }

        public IFormattingFluentSyntax UseWhitespaces(int spaces  = 4)
        {
            this.options.IndentChar = " ";
            this.options.IndentCount = spaces;
            return this;
        }

        public IFormattingFluentSyntax UseTab(int tabs = 1)
        {
            this.options.IndentChar = "\t";
            this.options.IndentCount = tabs;
            return this;
        }

        public IFormattingFluentSyntax Quotes(string quote)
        {
            this.options.Quote = quote;
            return this;
        }
    }
}
