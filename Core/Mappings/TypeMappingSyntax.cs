using KY.Generator.Languages;

namespace KY.Generator.Mappings
{
    internal class TypeMappingSyntax : ITypeMappingSyntax
    {
        private readonly TypeMapping mapping;
        private readonly IMappableLanguage fromLanguage;
        private readonly string fromType;

        public TypeMappingSyntax(TypeMapping mapping, IMappableLanguage fromLanguage, string fromType)
        {
            this.mapping = mapping;
            this.fromLanguage = fromLanguage;
            this.fromType = fromType;
        }

        public void To(IMappableLanguage toLanguage, string toType, string constructor = null)
        {
            this.mapping.Add(this.fromLanguage, this.fromType, toLanguage, toType, constructor: constructor);
        }
    }
}