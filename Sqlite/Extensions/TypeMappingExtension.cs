using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Language;
using KY.Generator.Sqlite.Helpers;
using KY.Generator.Sqlite.Language;

namespace KY.Generator.Sqlite.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Map(SqliteLanguage.Instance).To(CsharpLanguage.Instance)
                       .From(SqliteType.Real).To("double").FromSystem()
                       .From(SqliteType.Numeric).To("double").FromSystem()
                       .From(SqliteType.Integer).To("int").FromSystem()
                       .From(SqliteType.Text).To("string").Nullable().FromSystem()
                       .From(SqliteType.Blob).To("byte[]").Nullable().FromSystem();

            return typeMapping;
        }
    }
}
