using KY.Generator.Mappings;

namespace KY.Generator.Tsql.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(Code.Language, "smallint", Csharp.Code.Language, "short", true);
            typeMapping.Add(Code.Language, "bigint", Csharp.Code.Language, "long", true);
            typeMapping.Add(Code.Language, "bit", Csharp.Code.Language, "bool", true);
            typeMapping.Add(Code.Language, "int", Csharp.Code.Language, "int", true);
            typeMapping.Add(Code.Language, "date", Csharp.Code.Language, "DateTime", true, "System");
            typeMapping.Add(Code.Language, "datetime", Csharp.Code.Language, "DateTime", true, "System");
            typeMapping.Add(Code.Language, "datetime2", Csharp.Code.Language, "DateTime", true, "System");
            typeMapping.Add(Code.Language, "decimal", Csharp.Code.Language, "decimal", true);
            typeMapping.Add(Code.Language, "nchar", Csharp.Code.Language, "string");
            typeMapping.Add(Code.Language, "nvarchar", Csharp.Code.Language, "string");
            typeMapping.Add(Code.Language, "char", Csharp.Code.Language, "string");
            typeMapping.Add(Code.Language, "varchar", Csharp.Code.Language, "string");
            typeMapping.Add(Code.Language, "varbinary", Csharp.Code.Language, "byte[]");
            typeMapping.Add(Code.Language, "timestamp", Csharp.Code.Language, "byte[]");
            typeMapping.Add(Code.Language, "uniqueidentifier", Csharp.Code.Language, "Guid", false, "System");
            typeMapping.Add(Code.Language, "float", Csharp.Code.Language, "double", true);
            return typeMapping;
        }
    }
}
