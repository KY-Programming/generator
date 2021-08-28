using System;
using KY.Generator.Languages;
using KY.Generator.Transfer;

namespace KY.Generator.Mappings
{
    public interface ITypeMapping
    {
        ITypeMappingMapSyntax Map<T>() where T : ILanguage;
        TypeMappingEntry Add(Type fromLanguage, string fromType, Type toLanguage, string toType, bool nullable = false, string nameSpace = null, bool fromSystem = false, string constructor = null);
        TypeMappingEntry Get(ILanguage fromLanguage, string fromType, ILanguage toLanguage);
        TypeMappingEntry TryGet(ILanguage fromLanguage, string fromType, ILanguage toLanguage);
        TypeMappingEntry TryGet(ILanguage fromLanguage, Type fromType, ILanguage toLanguage);
        void Get(ILanguage fromLanguage, ILanguage toLanguage, TypeTransferObject type);
    }
}
