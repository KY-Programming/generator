using System;
using System.Collections.Generic;
using System.Linq;

namespace KY.Generator
{
    public class FormattingOptions
    {
        private readonly List<Func<FormattingOptions>> others;
        private string fileCase;
        private string classCase;
        private string fieldCase;
        private string propertyCase;
        private string methodCase;
        private string parameterCase;
        private string allowedSpecialCharacters;
        private string indentChar;
        private int? indentCount;
        private string quote;
        private string lineClosing;
        private string startBlock;
        private string endBlock;
        private bool? startBlockInNewLine;
        private bool? endFileWithNewLine;

        public string FileCase
        {
            get => this.Get(x => x?.fileCase) ?? Case.PascalCase;
            set => this.fileCase = value;
        }

        public string ClassCase
        {
            get => this.Get(x => x?.classCase) ?? Case.PascalCase;
            set => this.classCase = value;
        }

        public string FieldCase
        {
            get => this.Get(x => x?.fieldCase) ?? Case.CamelCase;
            set => this.fieldCase = value;
        }

        public string PropertyCase
        {
            get => this.Get(x => x?.propertyCase) ?? Case.PascalCase;
            set => this.propertyCase = value;
        }

        public string MethodCase
        {
            get => this.Get(x => x?.methodCase) ?? Case.PascalCase;
            set => this.methodCase = value;
        }

        public string ParameterCase
        {
            get => this.Get(x => x?.parameterCase) ?? Case.CamelCase;
            set => this.parameterCase = value;
        }

        public string AllowedSpecialCharacters
        {
            get => this.Get(x => x?.allowedSpecialCharacters);
            set => this.allowedSpecialCharacters = value;
        }

        public string IndentChar
        {
            get => this.Get(x => x?.indentChar) ?? " ";
            set => this.indentChar = value;
        }

        public int IndentCount
        {
            get => this.Get(x => x?.indentCount) ?? 4;
            set => this.indentCount = value;
        }

        public string Quote
        {
            get => this.Get(x => x?.quote) ?? "\"";
            set => this.quote = value;
        }

        public string LineClosing
        {
            get => this.Get(x => x?.lineClosing) ?? ";";
            set => this.lineClosing = value;
        }

        public string StartBlock
        {
            get => this.Get(x => x?.startBlock) ?? "{";
            set => this.startBlock = value;
        }

        public string EndBlock
        {
            get => this.Get(x => x?.endBlock) ?? "}";
            set => this.endBlock = value;
        }

        public bool StartBlockInNewLine
        {
            get => this.Get(x => x?.startBlockInNewLine) ?? true;
            set => this.startBlockInNewLine = value;
        }

        public bool EndFileWithNewLine
        {
            get => this.Get(x => x?.endFileWithNewLine) ?? true;
            set => this.endFileWithNewLine = value;
        }

        public FormattingOptions(params Func<FormattingOptions>[] others)
        {
            this.others = others.ToList();
        }

        private T Get<T>(Func<FormattingOptions, T> action)
            where T : class
        {
            return action(this) ?? this.others.Select(x => x()?.Get(action)).FirstOrDefault(x => x != null);
        }

        private T? Get<T>(Func<FormattingOptions, T?> action)
            where T : struct
        {
            return action(this) ?? this.others.Select(x => x()?.Get(action)).FirstOrDefault(x => x != null);
        }
    }
}
