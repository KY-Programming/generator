using KY.Generator.Languages;

namespace KY.Generator.Mappings
{
    public interface ITypeMappingSyntax
    {
        void To(IMappableLanguage language, string type, string constructor = null);
    }
}