using KY.Generator.Languages;

namespace KY.Generator.Mappings
{
    public class TypeMappingEntry
    {
        public ILanguage FromLanguage { get; }
        public string FromType { get; }
        public ILanguage ToLanguage { get; }
        public string ToType { get; }
        public bool Nullable { get; }
        public string Namespace { get; }
        public bool FromSystem { get; }
        public string Constructor { get; }

        public TypeMappingEntry(ILanguage fromLanguage, string fromType, ILanguage toLanguage, string toType, bool nullable, string nameSpace, bool fromSystem, string constructor = null)
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
}