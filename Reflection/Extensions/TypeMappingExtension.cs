
using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.Reflection.Language;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.Reflection.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(ReflectionLanguage.Instance, "System.Array", CsharpLanguage.Instance, "Array", nameSpace: "System", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Boolean", CsharpLanguage.Instance, "bool", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Byte", CsharpLanguage.Instance, "byte", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Char", CsharpLanguage.Instance, "char", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.DateTime", CsharpLanguage.Instance, "DateTime", nameSpace: "System", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Decimal", CsharpLanguage.Instance, "decimal", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Double", CsharpLanguage.Instance, "double", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Guid", CsharpLanguage.Instance, "Guid", nameSpace: "System", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int16", CsharpLanguage.Instance, "short", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int32", CsharpLanguage.Instance, "int", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int64", CsharpLanguage.Instance, "long", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Object", CsharpLanguage.Instance, "unknown", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Single", CsharpLanguage.Instance, "float", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.String", CsharpLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.TimeSpan", CsharpLanguage.Instance, "TimeSpan", nameSpace: "System", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt16", CsharpLanguage.Instance, "ushort", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt32", CsharpLanguage.Instance, "uint", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt64", CsharpLanguage.Instance, "ulong", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Void", CsharpLanguage.Instance, "void", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Array", CsharpLanguage.Instance, "Array", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.IList", CsharpLanguage.Instance, "IList", nameSpace: "System.Collections.Generic", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.List", CsharpLanguage.Instance, "List", nameSpace: "System.Collections.Generic", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.IEnumerable", CsharpLanguage.Instance, "IEnumerable", nameSpace: "System.Collections.Generic", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.Dictionary", CsharpLanguage.Instance, "Dictionary", nameSpace: "System.Collections.Generic", fromSystem: true);

            typeMapping.Add(ReflectionLanguage.Instance, "System.Array", TypeScriptLanguage.Instance, "Array", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Boolean", TypeScriptLanguage.Instance, "boolean", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Byte", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Char", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.DateTime", TypeScriptLanguage.Instance, "Date", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Decimal", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Double", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Guid", TypeScriptLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int16", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int32", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Int64", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Object", TypeScriptLanguage.Instance, "unknown", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Single", TypeScriptLanguage.Instance, "number", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.String", TypeScriptLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.TimeSpan", TypeScriptLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt16", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt32", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.UInt64", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Void", TypeScriptLanguage.Instance, "void", true, fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Array", TypeScriptLanguage.Instance, "Array", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.IList", TypeScriptLanguage.Instance, "Array", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.List", TypeScriptLanguage.Instance, "Array", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.IEnumerable", TypeScriptLanguage.Instance, "Array", fromSystem: true);
            typeMapping.Add(ReflectionLanguage.Instance, "System.Collections.Generic.Dictionary", TypeScriptLanguage.Instance, "Dictionary", fromSystem: true);
            return typeMapping;
        }
    }
}