using KY.Generator.Languages;

namespace KY.Generator.Mappings
{
    public interface ITypeMapping
    {
        ITypeMappingSyntax Map(ILanguage language, string type);
        void Add(ILanguage fromLanguage, string fromType, ILanguage toLanguage, string toType, bool nullable = false, string nameSpace = null, string constructor = null);
        TypeMappingEntry Get(ILanguage fromLanguage, string fromType, ILanguage toLanguage);
        TypeMappingEntry TryGet(ILanguage fromLanguage, string fromType, ILanguage toLanguage);
    }
}