namespace KY.Generator.Syntax
{
    public class FormattingFluentSyntax : IFormattingFluentSyntax
    {
        private readonly FormattingOptions options;

        public FormattingFluentSyntax(FormattingOptions options)
        {
            this.options = options;
        }

        public IFormattingFluentSyntax FileCase(string casing)
        {
            this.options.FileCase = casing;
            return this;
        }

        public IFormattingFluentSyntax ClassCase(string casing)
        {
            this.options.ClassCase = casing;
            return this;
        }

        public IFormattingFluentSyntax FieldCase(string casing)
        {
            this.options.FieldCase = casing;
            return this;
        }

        public IFormattingFluentSyntax PropertyCase(string casing)
        {
            this.options.PropertyCase = casing;
            return this;
        }

        public IFormattingFluentSyntax MethodCase(string casing)
        {
            this.options.MethodCase = casing;
            return this;
        }

        public IFormattingFluentSyntax ParameterCase(string casing)
        {
            this.options.ParameterCase = casing;
            return this;
        }

        public IFormattingFluentSyntax AllowedSpecialCharacters(string specialCharacters)
        {
            this.options.AllowedSpecialCharacters = specialCharacters;
            return this;
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

        public IFormattingFluentSyntax NoStartBlockInNewLine()
        {
            this.options.StartBlockInNewLine = false;
            return this;
        }

        public IFormattingFluentSyntax NoEndFileWithNewLine()
        {
            this.options.EndFileWithNewLine = false;
            return this;
        }

        public IFormattingFluentSyntax CollapseEmptyClasses()
        {
            this.options.CollapseEmptyClasses = true;
            return this;
        }

        public IFormattingFluentSyntax CollapsedClassesSpacer(string spacer)
        {
            this.options.CollapsedClassesSpacer = spacer;
            return this;
        }
    }
}
