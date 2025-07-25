﻿using KY.Core.Dependency;
using KY.Generator.Extensions;
using KY.Generator.Languages;
using KY.Generator.Templates;
using KY.Generator.TypeScript.Templates;
using KY.Generator.TypeScript.Writers;

namespace KY.Generator.TypeScript.Languages;

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
        this.Formatting.AllowedSpecialCharacters = "$";
        this.Formatting.StartBlockInNewLine = false;
        this.Formatting.FileCase = Case.KebabCase;
        this.Formatting.ClassCase = Case.PascalCase;
        this.Formatting.FieldCase = Case.CamelCase;
        this.Formatting.PropertyCase = Case.CamelCase;
        this.Formatting.MethodCase = Case.CamelCase;
        this.Formatting.ParameterCase = Case.CamelCase;
        this.Formatting.FileNameReplacer.Get("interface-prefix").SetReplacement("$1.interface");

        this.ReservedKeywords.Add("any", "anyValue");
        this.ReservedKeywords.Add("async", "asyncValue");
        this.ReservedKeywords.Add("await", "awaitValue");
        this.ReservedKeywords.Add("boolean", "booleanValue");
        this.ReservedKeywords.Add("constructor", "constructorValue");
        this.ReservedKeywords.Add("debugger", "debuggerValue");
        this.ReservedKeywords.Add("declare", "declareValue");
        this.ReservedKeywords.Add("delete", "deleteValue");
        this.ReservedKeywords.Add("export", "exportValue");
        this.ReservedKeywords.Add("extends", "extendsValue");
        this.ReservedKeywords.Add("from", "fromValue");
        this.ReservedKeywords.Add("function", "functionValue");
        this.ReservedKeywords.Add("get", "getValue");
        this.ReservedKeywords.Add("implements", "implementsValue");
        this.ReservedKeywords.Add("import", "importValue");
        this.ReservedKeywords.Add("instanceof", "instanceofValue");
        this.ReservedKeywords.Add("let", "letValue");
        this.ReservedKeywords.Add("module", "moduleValue");
        this.ReservedKeywords.Add("number", "numberValue");
        this.ReservedKeywords.Add("of", "ofValue");
        this.ReservedKeywords.Add("package", "packageValue");
        this.ReservedKeywords.Add("require", "requireValue");
        this.ReservedKeywords.Add("set", "setValue");
        this.ReservedKeywords.Add("string", "stringValue");
        this.ReservedKeywords.Add("super", "superValue");
        this.ReservedKeywords.Add("symbol", "symbolValue");
        this.ReservedKeywords.Add("type", "typeValue");
        this.ReservedKeywords.Add("var", "varValue");
        this.ReservedKeywords.Add("with", "withValue");
        this.ReservedKeywords.Add("yield", "yieldValue");

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
        this.AddWriter<TypeScriptUnionTypeTemplate, TypeScriptUnionTypeWriter>();
    }

    public override string FormatFile(string name, GeneratorOptions options, string type = null, bool force = false)
    {
        string fileName = base.FormatFile(name, options, type, force);
        // if ("interface".Equals(type, StringComparison.CurrentCultureIgnoreCase))
        // {
        //     fileName += ".interface";
        // }
        return fileName + ".ts";
    }
}
