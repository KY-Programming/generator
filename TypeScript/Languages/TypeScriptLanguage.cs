using System;
using System.Collections.Generic;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.TypeScript.Writers;
using KY.Generator.Writers;

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

        protected TypeScriptLanguage()
        {
            this.Formatting.StartBlockInNewLine = false;
            this.Formatting.EndFileWithNewLine = true;
            this.Formatting.FileCase = Case.KebabCase;
            this.Formatting.ClassCase = Case.PascalCase;
            this.Formatting.FieldCase = Case.CamelCase;
            this.Formatting.PropertyCase = Case.CamelCase;
            this.Formatting.MethodCase = Case.CamelCase;
            this.Formatting.ParameterCase = Case.CamelCase;

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
            this.TemplateWriters[typeof(ForceNullValueTemplate)] = new UndefinedValueWriter();
            this.TemplateWriters[typeof(NullTemplate)] = new UndefinedWriter();
            this.TemplateWriters[typeof(ForceNullTemplate)] = new UndefinedWriter();
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
            this.TemplateWriters[typeof(DateTimeTemplate)] = new TypeScriptDateTimeWriter();
            this.TemplateWriters[typeof(NumberTemplate)] = new TypeScriptNumberWriter();
            this.TemplateWriters[typeof(DeclareTypeTemplate)] = new DeclareTypeWriter();
        }

        protected override void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            fileTemplate.Header.Description += Environment.NewLine + "/* eslint-disable */" + Environment.NewLine + "tslint:disable";
            base.WriteHeader(fileTemplate, output);
        }

        public override string FormatFileName(string fileName, string fileType = null)
        {
            fileName = Formatter.Format(fileName, this.Formatting.FileCase, this.Formatting.AllowedSpecialCharacters);
            if (fileName.StartsWith("i-") || "interface".Equals(fileType, StringComparison.CurrentCultureIgnoreCase))
            {
                fileName = fileName.TrimStart("i-") + ".interface";
            }
            return fileName + ".ts";
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByTypeAndPath();
        }
    }
}