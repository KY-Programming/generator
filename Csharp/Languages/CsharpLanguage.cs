using KY.Core.Dependency;
using KY.Generator.Csharp.Templates;
using KY.Generator.Csharp.Writers;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Csharp.Languages;

public class CsharpLanguage : BaseLanguage
{
    public override string Name => "Csharp";
    public override bool ImportFromSystem => true;
    public override bool IsGenericTypeWithSameNameAllowed => true;

    public CsharpLanguage(IDependencyResolver resolver)
        : base(resolver)
    {
        this.Formatting.InterfacePrefix = "I";

        this.ReservedKeywords.Add("abstract", "abstractValue");
        this.ReservedKeywords.Add("base", "baseValue");
        this.ReservedKeywords.Add("bool", "boolValue");
        this.ReservedKeywords.Add("byte", "byteValue");
        this.ReservedKeywords.Add("char", "charValue");
        this.ReservedKeywords.Add("checked", "checkedValue");
        this.ReservedKeywords.Add("decimal", "decimalValue");
        this.ReservedKeywords.Add("delegate", "delegateValue");
        this.ReservedKeywords.Add("double", "doubleValue");
        this.ReservedKeywords.Add("event", "eventValue");
        this.ReservedKeywords.Add("explicit", "explicitValue");
        this.ReservedKeywords.Add("extern", "externValue");
        this.ReservedKeywords.Add("fixed", "fixedValue");
        this.ReservedKeywords.Add("float", "floatValue");
        this.ReservedKeywords.Add("foreach", "foreachValue");
        this.ReservedKeywords.Add("goto", "gotoValue");
        this.ReservedKeywords.Add("implicit", "implicitValue");
        this.ReservedKeywords.Add("int", "intValue");
        this.ReservedKeywords.Add("internal", "internalValue");
        this.ReservedKeywords.Add("is", "isValue");
        this.ReservedKeywords.Add("lock", "lockValue");
        this.ReservedKeywords.Add("long", "longValue");
        this.ReservedKeywords.Add("object", "objectValue");
        this.ReservedKeywords.Add("operator", "operatorValue");
        this.ReservedKeywords.Add("out", "outValue");
        this.ReservedKeywords.Add("override", "overrideValue");
        this.ReservedKeywords.Add("params", "paramsValue");
        this.ReservedKeywords.Add("readonly", "readonlyValue");
        this.ReservedKeywords.Add("ref", "refValue");
        this.ReservedKeywords.Add("sbyte", "sbyteValue");
        this.ReservedKeywords.Add("sealed", "sealedValue");
        this.ReservedKeywords.Add("short", "shortValue");
        this.ReservedKeywords.Add("sizeof", "sizeofValue");
        this.ReservedKeywords.Add("stackalloc", "stackallocValue");
        this.ReservedKeywords.Add("string", "stringValue");
        this.ReservedKeywords.Add("struct", "structValue");
        this.ReservedKeywords.Add("uint", "uintValue");
        this.ReservedKeywords.Add("ulong", "ulongValue");
        this.ReservedKeywords.Add("unchecked", "uncheckedValue");
        this.ReservedKeywords.Add("unsafe", "unsafeValue");
        this.ReservedKeywords.Add("ushort", "ushortValue");
        this.ReservedKeywords.Add("using", "usingValue");
        this.ReservedKeywords.Add("virtual", "virtualValue");
        this.ReservedKeywords.Add("volatile", "volatileValue");

        this.AddWriter<AttributeTemplate, AttributeWriter>();
        this.AddWriter<BaseTypeTemplate, BaseTypeWriter>();
        this.AddWriter<BaseTemplate, BaseWriter>();
        this.AddWriter<CastTemplate, CastWriter>();
        this.AddWriter<CommentTemplate, CommentWriter>();
        this.AddWriter<ConstructorTemplate, ConstructorWriter>();
        this.AddWriter<ConstraintTemplate, ConstraintWriter>();
        this.AddWriter<CsharpTemplate, CsharpWriter>();
        this.AddWriter<DeclareTemplate, DeclareWriter>();
        this.AddWriter<GenericTypeTemplate, CsharpGenericTypeWriter>();
        this.AddWriter<ParameterTemplate, ParameterWriter>();
        this.AddWriter<ThrowTemplate, ThrowWriter>();
        this.AddWriter<UsingTemplate, UsingWriter>();
        this.AddWriter<UsingDeclarationTemplate, UsingDeclarationWriter>();
        this.AddWriter<VerbatimStringTemplate, VerbatimStringWriter>();
        this.AddWriter<ClassTemplate, CsharpClassWriter>();
        this.AddWriter<YieldReturnTemplate, YieldReturnWriter>();
        this.AddWriter<FileTemplate, CsharpFileWriter>();
    }

    public override string FormatFile(string name, IOptions options, string type = null, bool force = false)
    {
        return base.FormatFile(name, options, type, force) + ".cs";
    }
}
