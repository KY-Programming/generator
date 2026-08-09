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
                       .From("tinyint").To("byte").Nullable()
                       .From("smallint").To("short").Nullable()
                       .From("bigint").To("long").Nullable()
                       .From("bit").To("bool").Nullable()
                       .From("int").To("int").Nullable()
                       .From("date").To("DateTime").Nullable().Namespace("System")
                       .From("datetime").To("DateTime").Nullable().Namespace("System")
                       .From("datetime2").To("DateTime").Nullable().Namespace("System")
                       .From("smalldatetime").To("DateTime").Nullable().Namespace("System")
                       .From("datetimeoffset").To("DateTimeOffset").Nullable().Namespace("System")
                       .From("time").To("TimeSpan").Nullable().Namespace("System")
                       .From("decimal").To("decimal").Nullable()
                       .From("numeric").To("decimal").Nullable()
                       .From("money").To("decimal").Nullable()
                       .From("smallmoney").To("decimal").Nullable()
                       .From("nchar").To("string")
                       .From("nvarchar").To("string")
                       .From("char").To("string")
                       .From("varchar").To("string")
                       .From("text").To("string")
                       .From("ntext").To("string")
                       .From("xml").To("string")
                       .From("binary").To("byte[]")
                       .From("varbinary").To("byte[]")
                       .From("image").To("byte[]")
                       .From("timestamp").To("byte[]")
                       .From("uniqueidentifier").To("Guid").Namespace("System")
                       .From("real").To("float").Nullable()
                       .From("float").To("double").Nullable();

            typeMapping.Map<TsqlLanguage>().To<TypeScriptLanguage>()
                       .From("tinyint").To("number").Nullable()
                       .From("smallint").To("number").Nullable()
                       .From("bigint").To("number").Nullable()
                       .From("bit").To("boolean").Nullable()
                       .From("int").To("number").Nullable()
                       .From("date").To("Date").Nullable()
                       .From("datetime").To("Date").Nullable()
                       .From("datetime2").To("Date").Nullable()
                       .From("smalldatetime").To("Date").Nullable()
                       // No native counterpart in TypeScript - these keep their T-SQL string representation
                       .From("datetimeoffset").To("string").Nullable()
                       .From("time").To("string").Nullable()
                       .From("decimal").To("number").Nullable()
                       .From("numeric").To("number").Nullable()
                       .From("money").To("number").Nullable()
                       .From("smallmoney").To("number").Nullable()
                       .From("nchar").To("string").Nullable()
                       .From("nvarchar").To("string").Nullable()
                       .From("char").To("string").Nullable()
                       .From("varchar").To("string").Nullable()
                       .From("text").To("string").Nullable()
                       .From("ntext").To("string").Nullable()
                       .From("xml").To("string").Nullable()
                       .From("binary").To("number[]").Nullable()
                       .From("varbinary").To("number[]").Nullable()
                       .From("image").To("number[]").Nullable()
                       .From("timestamp").To("number[]").Nullable()
                       .From("uniqueidentifier").To("string").Nullable()
                       .From("real").To("number").Nullable()
                       .From("float").To("number").Nullable();
            return typeMapping;
        }
    }
}
