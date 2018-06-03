using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.TypeScript.Writers;

namespace KY.Generator.TypeScript.Languages
{
    public class TypeScriptLanguage : BaseLanguage
    {
        public override string NamespaceKeyword => "export namespace";
        public override string ClassScope => "export";
        public override string PartialKeyword => string.Empty;
        public override string Name => "TypeScript";

        public TypeScriptLanguage()
        {
            this.Formatting.StartBlockInNewLine = false;
            this.TemplateWriters[typeof(BaseTypeTemplate)] = new BaseTypeWriter(this);
            this.TemplateWriters[typeof(BaseTemplate)] = new BaseWriter();
            this.TemplateWriters[typeof(CastTemplate)] = new CastWriter(this);
            this.TemplateWriters[typeof(ConstructorTemplate)] = new TypeScriptMethodWriter(this);
            this.TemplateWriters[typeof(DeclareTemplate)] = new DeclareWriter(this);
            this.TemplateWriters[typeof(FieldTemplate)] = new TypeScriptFieldWriter(this);
            this.TemplateWriters[typeof(GenericTypeTemplate)] = new TypeScriptGenericTypeWriter(this);
            this.TemplateWriters[typeof(MethodTemplate)] = new TypeScriptMethodWriter(this);
            this.TemplateWriters[typeof(NullValueTemplate)] = new UndefinedValueWriter();
            this.TemplateWriters[typeof(NullTemplate)] = new UndefinedWriter();
            this.TemplateWriters[typeof(ParameterTemplate)] = new ParameterWriter(this);
            this.TemplateWriters[typeof(PropertyTemplate)] = new TypeScriptPropertyWriter(this);
            this.TemplateWriters[typeof(ThrowTemplate)] = new ThrowWriter(this);
            this.TemplateWriters[typeof(EnumTemplate)] = new TypeScriptEnumWriter(this);
            this.TemplateWriters[typeof(OperatorTemplate)] = new TypeScriptOperatorWriter();
            this.TemplateWriters[typeof(TypeScriptTemplate)] = new TypeScriptWriter();
            this.TemplateWriters[typeof(UsingTemplate)] = new UsingWriter();
        }

        public override string GetFileName(FileTemplate fileTemplate)
        {
            //Ignore all files with all upercase like TEST_FILE
            if (fileTemplate.Name == fileTemplate.Name.ToUpperInvariant())
            {
                return fileTemplate.Name + ".ts";
            }
            ClassTemplate classTemplate = fileTemplate.Namespaces.FirstOrDefault()?.Children.OfType<ClassTemplate>().FirstOrDefault();
            string fileName = Regex.Replace(fileTemplate.Name, "[A-Z]", x => "-" + x.Value.ToLowerInvariant()).Trim('-');
            if (classTemplate != null && classTemplate.IsInterface)
            {
                if (fileName.StartsWith("i-"))
                {
                    fileName = fileName.Substring(2, fileName.Length - 2);
                }
                fileName += ".interface";
            }
            return fileName + ".ts";
        }

        public override string GetClassName(ClassTemplate classTemplate)
        {
            return classTemplate.Name == null ? null : Regex.Replace(classTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetClassName(EnumTemplate classTemplate)
        {
            return classTemplate.Name == null ? null : Regex.Replace(classTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetPropertyName(PropertyTemplate propertyTemplate)
        {
            return propertyTemplate.Name == null ? null : Regex.Replace(propertyTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetFieldName(FieldTemplate fieldTemplate)
        {
            return fieldTemplate.Name == null ? null : Regex.Replace(fieldTemplate.Name, "[^A-z0-9_]", "_");
        }

        public override string GetMethodName(MethodTemplate methodTemplate)
        {
            return methodTemplate.Name == null ? null : Regex.Replace(methodTemplate.Name, "[^A-z0-9_]", "_");
        }

        protected override IEnumerable<UsingTemplate> GetUsings(FileTemplate fileTemplate)
        {
            return fileTemplate.GetUsingsByTypeAndPath();
        }
    }
}