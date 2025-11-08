using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Extensions;
using KY.Generator.Models;

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
        private bool? collapseEmptyClasses;
        private string collapsedClassesSpacer;
        private string interfacePrefix;
        private string classPrefix;
        private readonly List<FileNameReplacer> fileNameReplacer = new();
        private CaseMode? caseMode;

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

        public bool CollapseEmptyClasses
        {
            get => this.Get(x => x?.collapseEmptyClasses) ?? false;
            set => this.collapseEmptyClasses = value;
        }

        public string CollapsedClassesSpacer
        {
            get => this.Get(x => x?.collapsedClassesSpacer) ?? " ";
            set => this.collapsedClassesSpacer = value;
        }

        public string InterfacePrefix
        {
            get => this.Get(x => x?.interfacePrefix) ?? "";
            set => this.interfacePrefix = value;
        }

        public string ClassPrefix
        {
            get => this.Get(x => x?.classPrefix) ?? "";
            set => this.classPrefix = value;
        }

        public IReadOnlyList<FileNameReplacer> FileNameReplacer => this.GetMerged(x => x?.fileNameReplacer);

        public CaseMode CaseMode
        {
            get => this.Get(x => x?.caseMode) ?? CaseMode.Fix;
            set => this.caseMode = value;
        }

        public FormattingOptions(params Func<FormattingOptions>[] others)
        {
            this.others = others.ToList();
        }

        public FormattingOptions Add(FileNameReplacer replacer)
        {
            this.fileNameReplacer.AddIfNotExists(replacer);
            return this;
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

        protected List<T> GetMerged<T>(Func<FormattingOptions, List<T>> getAction)
        {
            List<T> merged = new();
            foreach (List<T> list in this.Yield().Concat(this.others.Select(x => x())).Select(getAction).Where(x => x != null))
            {
                list.Where(item => !merged.Contains(item)).ForEach(item => merged.Add(item));
            }
            return merged;
        }

        public void AddFileNameReplace(FileNameReplacer newFileNameReplacer)
        {
            this.fileNameReplacer.Add(newFileNameReplacer);
        }
    }
}
