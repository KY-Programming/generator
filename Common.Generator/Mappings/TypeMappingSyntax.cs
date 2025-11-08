using KY.Generator.Templates;

namespace KY.Generator.Mappings;

internal class TypeMappingSyntax : ITypeMappingMapSyntax, ITypeMappingFromSyntax, ITypeMappingTypeOrToDetailsSyntax
{
    private readonly TypeMapping mapping;
    private readonly Type fromLanguage;
    private Type toLanguage;
    private string fromType;
    private TypeMappingEntry entry;

    public TypeMappingSyntax(TypeMapping mapping, Type fromLanguage)
    {
        this.mapping = mapping;
        this.fromLanguage = fromLanguage;
    }

    ITypeMappingTypeSyntax ITypeMappingMapSyntax.To<T>()
    {
        this.toLanguage = typeof(T);
        return this;
    }

    ITypeMappingFromSyntax ITypeMappingTypeSyntax.From(string type)
    {
        this.fromType = type;
        return this;
    }

    ITypeMappingTypeOrToDetailsSyntax ITypeMappingFromSyntax.To(string type, string constructor)
    {
        this.entry = this.mapping.Add(this.fromLanguage, this.fromType, this.toLanguage, type);
        return this;
    }

    ITypeMappingTypeOrToDetailsSyntax ITypeMappingTypeOrToDetailsSyntax.FromSystem()
    {
        this.entry.FromSystem = true;
        return this;
    }

    ITypeMappingTypeOrToDetailsSyntax ITypeMappingTypeOrToDetailsSyntax.Nullable()
    {
        this.entry.Nullable = true;
        return this;
    }

    ITypeMappingTypeOrToDetailsSyntax ITypeMappingTypeOrToDetailsSyntax.Namespace(string nameSpace)
    {
        this.entry.Namespace = nameSpace;
        return this;
    }

    ITypeMappingTypeOrToDetailsSyntax ITypeMappingTypeOrToDetailsSyntax.Default(ICodeFragment? code)
    {
        this.entry.Default = code;
        this.entry.StrictDefault = code;
        return this;
    }

    ITypeMappingTypeOrToDetailsSyntax ITypeMappingTypeOrToDetailsSyntax.Default(ICodeFragment? notStrictDefaultCode, ICodeFragment? strictDefaultCode)
    {
        this.entry.Default = notStrictDefaultCode;
        this.entry.StrictDefault = strictDefaultCode;
        return this;
    }
}
