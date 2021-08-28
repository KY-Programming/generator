using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Tsql.Language
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Map<TsqlLanguage>().To<CsharpLanguage>()
                       .From("smallint").To("short").Nullable()
                       .From("bigint").To("long").Nullable()
                       .From("bit").To("bool").Nullable()
                       .From("int").To("int").Nullable()
                       .From("date").To("DateTime").Nullable().Namespace("System")
                       .From("datetime").To("DateTime").Nullable().Namespace("System")
                       .From("datetime2").To("DateTime").Nullable().Namespace("System")
                       .From("decimal").To("decimal").Nullable()
                       .From("nchar").To("string")
                       .From("nvarchar").To("string")
                       .From("char").To("string")
                       .From("varchar").To("string")
                       .From("varbinary").To("byte[]")
                       .From("timestamp").To("byte[]")
                       .From("uniqueidentifier").To("Guid").Namespace("System")
                       .From("float").To("double").Nullable();

            typeMapping.Map<TsqlLanguage>().To<TypeScriptLanguage>()
                       .From("smallint").To("number").Nullable()
                       .From("bigint").To("number").Nullable()
                       .From("bit").To("boolean").Nullable()
                       .From("int").To("number").Nullable()
                       .From("date").To("Date").Nullable()
                       .From("datetime").To("Date").Nullable()
                       .From("datetime2").To("Date").Nullable()
                       .From("decimal").To("number").Nullable()
                       .From("nchar").To("string").Nullable()
                       .From("nvarchar").To("string").Nullable()
                       .From("char").To("string").Nullable()
                       .From("varchar").To("string").Nullable()
                       .From("varbinary").To("number[]").Nullable()
                       .From("timestamp").To("number[]").Nullable()
                       .From("uniqueidentifier").To("string").Nullable()
                       .From("float").To("number").Nullable();
            return typeMapping;
        }
    }
}
