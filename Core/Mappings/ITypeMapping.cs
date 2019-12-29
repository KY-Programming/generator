using System;
using KY.Generator.Languages;
using KY.Generator.Transfer;

namespace KY.Generator.Mappings
{
    public interface ITypeMapping
    {
        ITypeMappingSyntax Map(IMappableLanguage language, string type);
        void Add(IMappableLanguage fromLanguage, string fromType, IMappableLanguage toLanguage, string toType, bool nullable = false, string nameSpace = null, bool fromSystem = false, string constructor = null);
        TypeMappingEntry Get(IMappableLanguage fromLanguage, string fromType, IMappableLanguage toLanguage);
        TypeMappingEntry TryGet(IMappableLanguage fromLanguage, string fromType, IMappableLanguage toLanguage);
        TypeMappingEntry TryGet(IMappableLanguage fromLanguage, Type fromType, IMappableLanguage toLanguage);
        void Get(IMappableLanguage fromLanguage, IMappableLanguage toLanguage, TypeTransferObject type);
    }
}