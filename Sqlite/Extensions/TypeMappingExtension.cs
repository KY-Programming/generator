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
            typeMapping.Add(SqliteLanguage.Instance, SqliteType.Real, CsharpLanguage.Instance, "double", fromSystem: true);
            typeMapping.Add(SqliteLanguage.Instance, SqliteType.Numeric, CsharpLanguage.Instance, "double", fromSystem: true);
            typeMapping.Add(SqliteLanguage.Instance, SqliteType.Integer, CsharpLanguage.Instance, "int", fromSystem: true);
            typeMapping.Add(SqliteLanguage.Instance, SqliteType.Text, CsharpLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(SqliteLanguage.Instance, SqliteType.Blob, CsharpLanguage.Instance, "byte[]", fromSystem: true);

            typeMapping.Add(ReflectionLanguage.Instance, "System.Boolean", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Byte", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Char", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.DateTime", SqliteLanguage.Instance, SqliteType.Text, true, nameSpace: "System", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Decimal", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Double", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Guid", SqliteLanguage.Instance, SqliteType.Text, true, nameSpace: "System", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int16", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int32", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int64", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Single", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.String", SqliteLanguage.Instance, SqliteType.Text, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.TimeSpan", SqliteLanguage.Instance, SqliteType.Text, true, nameSpace: "System", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt16", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt32", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt64", SqliteLanguage.Instance, SqliteType.Integer, true, fromSystem: true);

            return typeMapping;
        }
    }
}
