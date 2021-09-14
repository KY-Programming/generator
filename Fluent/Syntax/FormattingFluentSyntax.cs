using System;
using System.Linq;
using KY.Generator.Extensions;
using KY.Generator.Models;

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

        public IFormattingFluentSyntax CollapseEmptyClasses(string spacer = " ")
        {
            this.options.CollapseEmptyClasses = true;
            this.options.CollapsedClassesSpacer = spacer;
            return this;
        }

        public IFormattingFluentSyntax ClassPrefix(string prefix)
        {
            this.options.ClassPrefix = prefix;
            return this;
        }

        public IFormattingFluentSyntax InterfacePrefix(string prefix)
        {
            this.options.InterfacePrefix = prefix;
            return this;
        }

        public IFormattingFluentSyntax AddFileNameReplacer(string key, string pattern, string replacement, string matchingType = null)
        {
            if (this.options.FileNameReplacer.Any(x => x.Key == key))
            {
                throw new InvalidOperationException($"FileNameReplace {key} already exists. Use {nameof(this.SetFileNameReplacer)} instead");
            }
            this.options.AddFileNameReplace(new FileNameReplacer(key, pattern, replacement, matchingType));
            return this;
        }

        public IFormattingFluentSyntax SetFileNameReplacer(string key, string replacement)
        {
            FileNameReplacer found = this.options.FileNameReplacer.FirstOrDefault(x => x.Key == key);
            if (found == null)
            {
                throw new InvalidOperationException($"FileNameReplace {key} does not exists. Use {nameof(this.AddFileNameReplacer)} first");
            }
            found.SetReplacement(replacement);
            return this;
        }

        public IFormattingFluentSyntax AddOrSetFileNameReplacer(string key, string pattern, string replacement, string matchingType = null)
        {
            FileNameReplacer found = this.options.FileNameReplacer.FirstOrDefault(x => x.Key == key);
            if (found == null)
            {
                this.options.AddFileNameReplace(new FileNameReplacer(key, pattern, replacement, matchingType));
            }
            else
            {
                found.SetPattern(pattern).SetReplacement(replacement).SetMatchingType(matchingType);
            }
            return this;
        }
    }
}
