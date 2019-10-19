using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.Tsql.Language;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Tsql.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(TsqlLanguage.Instance, "smallint", CsharpLanguage.Instance, "short", true);
            typeMapping.Add(TsqlLanguage.Instance, "bigint", CsharpLanguage.Instance, "long", true);
            typeMapping.Add(TsqlLanguage.Instance, "bit", CsharpLanguage.Instance, "bool", true);
            typeMapping.Add(TsqlLanguage.Instance, "int", CsharpLanguage.Instance, "int", true);
            typeMapping.Add(TsqlLanguage.Instance, "date", CsharpLanguage.Instance, "DateTime", true, "System");
            typeMapping.Add(TsqlLanguage.Instance, "datetime", CsharpLanguage.Instance, "DateTime", true, "System");
            typeMapping.Add(TsqlLanguage.Instance, "datetime2", CsharpLanguage.Instance, "DateTime", true, "System");
            typeMapping.Add(TsqlLanguage.Instance, "decimal", CsharpLanguage.Instance, "decimal", true);
            typeMapping.Add(TsqlLanguage.Instance, "nchar", CsharpLanguage.Instance, "string");
            typeMapping.Add(TsqlLanguage.Instance, "nvarchar", CsharpLanguage.Instance, "string");
            typeMapping.Add(TsqlLanguage.Instance, "char", CsharpLanguage.Instance, "string");
            typeMapping.Add(TsqlLanguage.Instance, "varchar", CsharpLanguage.Instance, "string");
            typeMapping.Add(TsqlLanguage.Instance, "varbinary", CsharpLanguage.Instance, "byte[]");
            typeMapping.Add(TsqlLanguage.Instance, "timestamp", CsharpLanguage.Instance, "byte[]");
            typeMapping.Add(TsqlLanguage.Instance, "uniqueidentifier", CsharpLanguage.Instance, "Guid", false, "System");
            typeMapping.Add(TsqlLanguage.Instance, "float", CsharpLanguage.Instance, "double", true);

            typeMapping.Add(TsqlLanguage.Instance, "smallint", TypeScriptLanguage.Instance, "number", true);
            typeMapping.Add(TsqlLanguage.Instance, "bigint", TypeScriptLanguage.Instance, "number", true);
            typeMapping.Add(TsqlLanguage.Instance, "bit", TypeScriptLanguage.Instance, "boolean", true);
            typeMapping.Add(TsqlLanguage.Instance, "int", TypeScriptLanguage.Instance, "number", true);
            typeMapping.Add(TsqlLanguage.Instance, "date", TypeScriptLanguage.Instance, "Date", true);
            typeMapping.Add(TsqlLanguage.Instance, "datetime", TypeScriptLanguage.Instance, "Date", true);
            typeMapping.Add(TsqlLanguage.Instance, "datetime2", TypeScriptLanguage.Instance, "Date", true);
            typeMapping.Add(TsqlLanguage.Instance, "decimal", TypeScriptLanguage.Instance, "number", true);
            typeMapping.Add(TsqlLanguage.Instance, "nchar", TypeScriptLanguage.Instance, "string", true);
            typeMapping.Add(TsqlLanguage.Instance, "nvarchar", TypeScriptLanguage.Instance, "string", true);
            typeMapping.Add(TsqlLanguage.Instance, "char", TypeScriptLanguage.Instance, "string", true);
            typeMapping.Add(TsqlLanguage.Instance, "varchar", TypeScriptLanguage.Instance, "string", true);
            typeMapping.Add(TsqlLanguage.Instance, "varbinary", TypeScriptLanguage.Instance, "number[]", true);
            typeMapping.Add(TsqlLanguage.Instance, "timestamp", TypeScriptLanguage.Instance, "number[]", true);
            typeMapping.Add(TsqlLanguage.Instance, "uniqueidentifier", TypeScriptLanguage.Instance, "string", true);
            typeMapping.Add(TsqlLanguage.Instance, "float", TypeScriptLanguage.Instance, "number", true);
            return typeMapping;
        }
    }
}
