using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.TypeScript.Writers;

namespace KY.Generator.TypeScript.Languages
{
    public class TypeScriptLanguage : BaseLanguage
    {
        public static TypeScriptLanguage Instance { get; } = new TypeScriptLanguage();

        public override string NamespaceKeyword => "export namespace";
        public override string ClassScope => "export";
        public override string PartialKeyword => string.Empty;
        public override string Name => "TypeScript";
        public override bool ImportFromSystem => false;

        private TypeScriptLanguage()
        {
            this.Formatting.StartBlockInNewLine = false;
            this.Formatting.EndFileWithNewLine = true;
            this.HasStaticClasses = false;
            this.TemplateWriters[typeof(BaseTypeTemplate)] = new BaseTypeWriter();
            this.TemplateWriters[typeof(BaseTemplate)] = new BaseWriter();
            this.TemplateWriters[typeof(CastTemplate)] = new CastWriter();
            this.TemplateWriters[typeof(ConstructorTemplate)] = new TypeScriptMethodWriter();
            this.TemplateWriters[typeof(DeclareTemplate)] = new DeclareWriter();
            this.TemplateWriters[typeof(FieldTemplate)] = new TypeScriptFieldWriter();
            this.TemplateWriters[typeof(GenericTypeTemplate)] = new TypeScriptGenericTypeWriter();
            this.TemplateWriters[typeof(MethodTemplate)] = new TypeScriptMethodWriter();
            this.TemplateWriters[typeof(ExtensionMethodTemplate)] = new TypeScriptMethodWriter();
            this.TemplateWriters[typeof(NullValueTemplate)] = new UndefinedValueWriter();
            this.TemplateWriters[typeof(NullTemplate)] = new UndefinedWriter();
            this.TemplateWriters[typeof(ParameterTemplate)] = new ParameterWriter();
            this.TemplateWriters[typeof(PropertyTemplate)] = new TypeScriptPropertyWriter();
            this.TemplateWriters[typeof(ThrowTemplate)] = new ThrowWriter();
            this.TemplateWriters[typeof(EnumTemplate)] = new TypeScriptEnumWriter();
            this.TemplateWriters[typeof(OperatorTemplate)] = new TypeScriptOperatorWriter();
            this.TemplateWriters[typeof(TypeScriptTemplate)] = new TypeScriptWriter();
            this.TemplateWriters[typeof(UsingTemplate)] = new UsingWriter();
            this.TemplateWriters[typeof(AttributeTemplate)] = new AttributeWriter(this);
            this.TemplateWriters[typeof(AnonymousObjectTemplate)] = new AnonymousObjectWriter();
            this.TemplateWriters[typeof(TypeTemplate)] = new TypeScriptTypeWriter();
        }

        protected override void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            fileTemplate.Header.Description += Environment.NewLine + "tslint:disable";
            base.WriteHeader(fileTemplate, output);
        }

        public override string FormatFileName(string fileName, bool isInterface)
        {
            return Code.GetFileName(fileName, isInterface);
        }

        public override string FormatClassName(string className)
        {
            return className == null ? null : Regex.Replace(className, "[^A-z0-9_]", "_").FirstCharToUpper();
        }

        public override string FormatPropertyName(string propertyName)
        {
            return propertyName == null ? null : Regex.Replace(propertyName, "[^A-z0-9_]", "_").FirstCharToLower();
        }

        public override string FormatFieldName(string fieldName)
        {
            return fieldName == null ? null : Regex.Replace(fieldName, "[^A-z0-9_]", "_").FirstCharToLower();
        }

        public override string FormatMethodName(string methodName)
        {
            return methodName == null ? null : Regex.Replace(methodName, "[^A-z0-9_]", "_").FirstCharToLower();
        }

        public override string FormatParameterName(string parameterName)
        {
            return parameterName == null ? null : Regex.Replace(parameterName, "[^A-z0-9_]", "_").FirstCharToLower();
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByTypeAndPath();
        }
    }
}