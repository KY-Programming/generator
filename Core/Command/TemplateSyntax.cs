using KY.Core;
using KY.Generator.Templates;

namespace KY.Generator.Command
{
    public class TemplateSyntax
    {
        private readonly string templateName;
        private string temlate;

        public TemplateSyntax(string templateName, string temlate)
        {
            this.templateName = templateName;
            this.temlate = temlate;
        }

        public TemplateSyntax SetVariable(string variableName, string variableValue)
        {
            this.temlate = this.temlate.Replace($"${variableName}$", variableValue);
            return this;
        }

        public override string ToString()
        {
            return this.temlate;
        }

        public StaticFileTemplate ToFile(string fileName, string relativePath, bool addHeader = true)
        {
            return new StaticFileTemplate(fileName ?? this.templateName.TrimEnd(".template").TrimEnd("template"), this.temlate, relativePath, addHeader);
        }

        public StaticFileTemplate ToFile(string relativePath, bool addHeader = true)
        {
            return this.ToFile(null, relativePath, addHeader);
        }

        public StaticFileTemplate ToFile(bool addHeader = true)
        {
            return this.ToFile(null, addHeader);
        }
    }
}