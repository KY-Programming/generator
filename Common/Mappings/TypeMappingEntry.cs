using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Mappings
{
    public class TypeMappingEntry
    {
        public IMappableLanguage FromLanguage { get; }
        public string FromType { get; }
        public IMappableLanguage ToLanguage { get; }
        public string ToType { get; }
        public bool Nullable { get; set; }
        public string Namespace { get; set; }
        public bool FromSystem { get; set; }
        public string Constructor { get; }
        public ICodeFragment Default { get; set; }

        public TypeMappingEntry(IMappableLanguage fromLanguage, string fromType, IMappableLanguage toLanguage, string toType, bool nullable, string nameSpace, bool fromSystem, string constructor = null)
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