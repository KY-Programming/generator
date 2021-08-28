using System;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.TypeScript.Writers;

namespace KY.Generator.TypeScript.Languages
{
    public class TypeScriptLanguage : BaseLanguage
    {
        public override string ClassScope => "export";
        public override string PartialKeyword => string.Empty;
        public override string Name => "TypeScript";
        public override bool ImportFromSystem => false;
        public override bool IsGenericTypeWithSameNameAllowed => false;

        public TypeScriptLanguage(IDependencyResolver resolver)
            : base(resolver)
        {
            this.Formatting.StartBlockInNewLine = false;
            this.Formatting.FileCase = Case.KebabCase;
            this.Formatting.ClassCase = Case.PascalCase;
            this.Formatting.FieldCase = Case.CamelCase;
            this.Formatting.PropertyCase = Case.CamelCase;
            this.Formatting.MethodCase = Case.CamelCase;
            this.Formatting.ParameterCase = Case.CamelCase;

            this.ReservedKeywords.Add("function", "func");
            this.HasStaticClasses = false;

            this.AddWriter<BaseTypeTemplate, BaseTypeWriter>();
            this.AddWriter<BaseTemplate, BaseWriter>();
            this.AddWriter<CastTemplate, CastWriter>();
            this.AddWriter<ConstructorTemplate, TypeScriptMethodWriter>();
            this.AddWriter<DeclareTemplate, DeclareWriter>();
            this.AddWriter<FieldTemplate, TypeScriptFieldWriter>();
            this.AddWriter<GenericTypeTemplate, TypeScriptGenericTypeWriter>();
            this.AddWriter<MethodTemplate, TypeScriptMethodWriter>();
            this.AddWriter<ExtensionMethodTemplate, TypeScriptMethodWriter>();
            this.AddWriter<NullValueTemplate, UndefinedValueWriter>();
            this.AddWriter<ForceNullValueTemplate, UndefinedValueWriter>();
            this.AddWriter<NullTemplate, UndefinedWriter>();
            this.AddWriter<ForceNullTemplate, UndefinedWriter>();
            this.AddWriter<ParameterTemplate, TypeScriptParameterWriter>();
            this.AddWriter<PropertyTemplate, TypeScriptPropertyWriter>();
            this.AddWriter<ThrowTemplate, ThrowWriter>();
            this.AddWriter<EnumTemplate, TypeScriptEnumWriter>();
            this.AddWriter<OperatorTemplate, TypeScriptOperatorWriter>();
            this.AddWriter<TypeScriptTemplate, TypeScriptWriter>();
            this.AddWriter<UsingTemplate, UsingWriter>();
            this.AddWriter<AttributeTemplate, AttributeWriter>();
            this.AddWriter<AnonymousObjectTemplate, AnonymousObjectWriter>();
            this.AddWriter<TypeTemplate, TypeScriptTypeWriter>();
            this.AddWriter<DateTimeTemplate, TypeScriptDateTimeWriter>();
            this.AddWriter<NumberTemplate, TypeScriptNumberWriter>();
            this.AddWriter<DeclareTypeTemplate, DeclareTypeWriter>();
            this.AddWriter<NamespaceTemplate, TypeScriptNamespaceWriter>();
            this.AddWriter<FileTemplate, TypeScriptFileWriter>();
        }

        public override string FormatFile(string name, IOptions options, string type = null, bool force = false)
        {
            string fileName = base.FormatFile(name, options, type, force);
            if (fileName.StartsWith("i-") /*|| "interface".Equals(fileType, StringComparison.CurrentCultureIgnoreCase)*/)
            {
                fileName = fileName.TrimStart("i-") + ".interface";
            }
            return fileName + ".ts";
        }
    }
}
