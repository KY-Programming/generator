using System;
using System.Collections.Generic;
using System.Linq;

namespace KY.Generator
{
    public class FormattingOptions
    {
        private readonly List<FormattingOptions> others;
        private string fileCase;
        private string classCase;
        private string fieldCase;
        private string propertyCase;
        private string methodCase;
        private string parameterCase;
        private string allowedSpecialCharacters;

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
            get => this.Get(x => x?.fieldCase) ?? Case.PascalCase;
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

        public FormattingOptions(params FormattingOptions[] others)
        {
            this.others = others.ToList();
        }

        private T Get<T>(Func<FormattingOptions, T> action)
            where T : class
        {
            return action(this) ?? this.others.Select(x => x?.Get(action)).FirstOrDefault(x => x != null);
        }
    }
}
