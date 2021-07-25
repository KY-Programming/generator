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

            typeMapping.Map(ReflectionLanguage.Instance).To(SqliteLanguage.Instance)
                       .From("System.Boolean").To(SqliteType.Integer)
                       .From("System.Byte").To(SqliteType.Integer)
                       .From("System.Char").To(SqliteType.Integer)
                       .From("System.DateTime").To(SqliteType.Text)
                       .From("System.Decimal").To(SqliteType.Integer)
                       .From("System.Double").To(SqliteType.Integer)
                       .From("System.Guid").To(SqliteType.Text)
                       .From("System.Int16").To(SqliteType.Integer)
                       .From("System.Int32").To(SqliteType.Integer)
                       .From("System.Int64").To(SqliteType.Integer)
                       .From("System.Single").To(SqliteType.Integer)
                       .From("System.String").To(SqliteType.Text)
                       .From("System.TimeSpan").To(SqliteType.Text)
                       .From("System.UInt16").To(SqliteType.Integer)
                       .From("System.UInt32").To(SqliteType.Integer)
                       .From("System.UInt64").To(SqliteType.Integer);

            return typeMapping;
        }
    }
}
