using KY.Generator.Templates;

namespace KY.Generator.Mappings;

public class TypeMappingEntry
{
    public Type FromLanguage { get; }
    public string FromType { get; }
    public Type ToLanguage { get; }
    public string ToType { get; }
    public bool Nullable { get; set; }
    public string Namespace { get; set; }
    public bool FromSystem { get; set; }
    public string Constructor { get; }
    public ICodeFragment? Default { get; set; }
    public ICodeFragment? StrictDefault { get; set; }

    public TypeMappingEntry(Type fromLanguage, string fromType, Type toLanguage, string toType, bool nullable, string nameSpace, bool fromSystem, string? constructor = null)
    {
        this.FromLanguage = fromLanguage;
        this.FromType = fromType;
        this.ToLanguage = toLanguage;
        this.ToType = toType;
        this.Nullable = nullable;
        this.Namespace = nameSpace;
        this.FromSystem = fromSystem;
        this.Constructor = constructor ?? this.ToType;
    }
}
