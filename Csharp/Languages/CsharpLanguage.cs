using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Generator.Csharp.Templates;
using KY.Generator.Csharp.Writers;
using KY.Generator.Languages;
using KY.Generator.Output;
using KY.Generator.Templates;

namespace KY.Generator.Csharp.Languages
{
    public class CsharpLanguage : BaseLanguage
    {
        public static CsharpLanguage Instance { get; } = new CsharpLanguage();

        public override string Name => "Csharp";
        public override bool ImportFromSystem => true;

        private CsharpLanguage()
        {
            this.TemplateWriters[typeof(AttributeTemplate)] = new AttributeWriter(this);
            this.TemplateWriters[typeof(BaseTypeTemplate)] = new BaseTypeWriter();
            this.TemplateWriters[typeof(BaseTemplate)] = new BaseWriter();
            this.TemplateWriters[typeof(CastTemplate)] = new CastWriter();
            this.TemplateWriters[typeof(CommentTemplate)] = new CommentWriter();
            this.TemplateWriters[typeof(ConstructorTemplate)] = new ConstructorWriter();
            this.TemplateWriters[typeof(ConstraintTemplate)] = new ConstraintWriter();
            this.TemplateWriters[typeof(CsharpTemplate)] = new CsharpWriter();
            this.TemplateWriters[typeof(DeclareTemplate)] = new DeclareWriter();
            this.TemplateWriters[typeof(NullCoalescingOperatorTemplate)] = new NullCoalescingOperatorWriter();
            this.TemplateWriters[typeof(ParameterTemplate)] = new ParameterWriter();
            this.TemplateWriters[typeof(ThrowTemplate)] = new ThrowWriter();
            this.TemplateWriters[typeof(UsingTemplate)] = new UsingWriter();
        }

        protected override void WriteHeader(FileTemplate fileTemplate, IOutputCache output)
        {
            fileTemplate.Header.Description += Environment.NewLine + "ReSharper disable All";
            base.WriteHeader(fileTemplate, output);
        }

        public override string FormatFileName(string fileName, bool isInterface)
        {
            return fileName.FirstCharToUpper() + ".cs";
        }

        public override string FormatClassName(string className)
        {
            return className == null ? null : Regex.Replace(className, "[^A-z0-9_]", "_").FirstCharToUpper();
        }

        public override string FormatPropertyName(string propertyName)
        {
            return propertyName == null ? null : Regex.Replace(propertyName, "[^A-z0-9_]", "_").FirstCharToUpper();
        }

        public override string FormatFieldName(string fieldName)
        {
            return fieldName == null ? null : Regex.Replace(fieldName, "[^A-z0-9_]", "_").FirstCharToLower();
        }

        public override string FormatMethodName(string methodName)
        {
            return methodName == null ? null : Regex.Replace(methodName, "[^A-z0-9_]", "_").FirstCharToUpper();
        }

        public override string FormatParameterName(string parameterName)
        {
            return parameterName == null ? null : Regex.Replace(parameterName, "[^A-z0-9_]", "_").FirstCharToLower();
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByNamespace();
        }
    }
}