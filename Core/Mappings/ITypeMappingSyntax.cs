using KY.Generator.Languages;

namespace KY.Generator.Mappings
{
    public interface ITypeMappingSyntax
    {
        void To(ILanguage language, string type, string constructor = null);
    }
}