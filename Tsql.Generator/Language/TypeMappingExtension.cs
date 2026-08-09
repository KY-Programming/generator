using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.TypeScript;
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
                       // Nullable() on a reference type as well: a column that allows NULL becomes "string?", and
                       // the file writer adds the "#nullable enable" such an annotation needs
                       .From("nchar").To("string").Nullable()
                       .From("nvarchar").To("string").Nullable()
                       .From("char").To("string").Nullable()
                       .From("varchar").To("string").Nullable()
                       .From("text").To("string").Nullable()
                       .From("ntext").To("string").Nullable()
                       .From("xml").To("string").Nullable()
                       .From("binary").To("byte[]").Nullable()
                       .From("varbinary").To("byte[]").Nullable()
                       .From("image").To("byte[]").Nullable()
                       .From("timestamp").To("byte[]").Nullable()
                       .From("uniqueidentifier").To("Guid").Nullable().Namespace("System")
                       .From("real").To("float").Nullable()
                       .From("float").To("double").Nullable();

            // The defaults are what a non nullable column is initialized with in TypeScript strict mode. Without
            // them a "NOT NULL" column would come out as "| undefined", which is exactly what it is not.
            typeMapping.Map<TsqlLanguage>().To<TypeScriptLanguage>()
                       .From("tinyint").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("smallint").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("bigint").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("bit").To("boolean").Nullable().Default(Code.Instance.Boolean(false))
                       .From("int").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("date").To("Date").Nullable().Default(Code.Instance.New(Code.Instance.Type("Date"), Code.Instance.Number(0)))
                       .From("datetime").To("Date").Nullable().Default(Code.Instance.New(Code.Instance.Type("Date"), Code.Instance.Number(0)))
                       .From("datetime2").To("Date").Nullable().Default(Code.Instance.New(Code.Instance.Type("Date"), Code.Instance.Number(0)))
                       .From("smalldatetime").To("Date").Nullable().Default(Code.Instance.New(Code.Instance.Type("Date"), Code.Instance.Number(0)))
                       // No native counterpart in TypeScript - these keep their T-SQL string representation
                       .From("datetimeoffset").To("string").Nullable().Default(null, Code.Instance.String("0001-01-01T00:00:00+00:00"))
                       .From("time").To("string").Nullable().Default(null, Code.Instance.String("00:00:00"))
                       .From("decimal").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("numeric").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("money").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("smallmoney").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("nchar").To("string").Nullable().Default(null, Code.Instance.String(string.Empty))
                       .From("nvarchar").To("string").Nullable().Default(null, Code.Instance.String(string.Empty))
                       .From("char").To("string").Nullable().Default(null, Code.Instance.String(string.Empty))
                       .From("varchar").To("string").Nullable().Default(null, Code.Instance.String(string.Empty))
                       .From("text").To("string").Nullable().Default(null, Code.Instance.String(string.Empty))
                       .From("ntext").To("string").Nullable().Default(null, Code.Instance.String(string.Empty))
                       .From("xml").To("string").Nullable().Default(null, Code.Instance.String(string.Empty))
                       .From("binary").To("number[]").Nullable().Default(Code.Instance.TypeScript("[]"))
                       .From("varbinary").To("number[]").Nullable().Default(Code.Instance.TypeScript("[]"))
                       .From("image").To("number[]").Nullable().Default(Code.Instance.TypeScript("[]"))
                       .From("timestamp").To("number[]").Nullable().Default(Code.Instance.TypeScript("[]"))
                       .From("uniqueidentifier").To("string").Nullable().Default(null, Code.Instance.String("00000000-0000-0000-0000-000000000000"))
                       .From("real").To("number").Nullable().Default(Code.Instance.Number(0))
                       .From("float").To("number").Nullable().Default(Code.Instance.Number(0));
            return typeMapping;
        }
    }
}
