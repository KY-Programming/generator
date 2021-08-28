using KY.Generator.Languages;

namespace KY.Generator.Mappings
{
    public interface ITypeMappingMapSyntax
    {
        ITypeMappingTypeSyntax To<T>() where T : ILanguage;
    }
}
