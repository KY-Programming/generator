using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Mappings
{
    internal class TypeMappingSyntax : ITypeMappingMapSyntax, ITypeMappingFromSyntax, ITypeMappingTypeOrToDetailsSyntax
    {
        private readonly TypeMapping mapping;
        private readonly IMappableLanguage fromLanguage;
        private IMappableLanguage toLanguage;
        private string fromType;
        private TypeMappingEntry entry;

        public TypeMappingSyntax(TypeMapping mapping, IMappableLanguage fromLanguage)
        {
            this.mapping = mapping;
            this.fromLanguage = fromLanguage;
        }

        ITypeMappingTypeSyntax ITypeMappingMapSyntax.To(IMappableLanguage language)
        {
            this.toLanguage = language;
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

        ITypeMappingTypeOrToDetailsSyntax ITypeMappingTypeOrToDetailsSyntax.Default(ICodeFragment code)
        {
            this.entry.Default = code;
            return this;
        }
    }
}
