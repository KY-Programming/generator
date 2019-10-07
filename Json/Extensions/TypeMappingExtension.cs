using KY.Generator.Csharp.Languages;
using KY.Generator.Json.Language;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.TypeScript.Languages;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Json.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Boolean.ToString(), CsharpLanguage.Instance, "bool", fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Bytes.ToString(), CsharpLanguage.Instance, "byte", fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Integer.ToString(), CsharpLanguage.Instance, "int", fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Float.ToString(), CsharpLanguage.Instance, "double", fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.String.ToString(), CsharpLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Date.ToString(), CsharpLanguage.Instance, "DateTime", false, "System", true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Guid.ToString(), CsharpLanguage.Instance, "Guid", false, "System", true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Uri.ToString(), CsharpLanguage.Instance, "Uri", false, "System", true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.TimeSpan.ToString(), CsharpLanguage.Instance, "TimeSpan", false, "System", true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Array.ToString(), CsharpLanguage.Instance, "List", false, "System.Collections.Generic", true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Object.ToString(), CsharpLanguage.Instance, "object");

            typeMapping.Add(JsonLanguage.Instance, JTokenType.Boolean.ToString(), TypeScriptLanguage.Instance, "boolean", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Bytes.ToString(), TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Integer.ToString(), TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Float.ToString(), TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.String.ToString(), TypeScriptLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Date.ToString(), TypeScriptLanguage.Instance, "Date", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Guid.ToString(), TypeScriptLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Uri.ToString(), TypeScriptLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.TimeSpan.ToString(), TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Array.ToString(), TypeScriptLanguage.Instance, "Array", true, fromSystem: true);
            typeMapping.Add(JsonLanguage.Instance, JTokenType.Object.ToString(), TypeScriptLanguage.Instance, "Object", true, fromSystem: true);
            return typeMapping;
        }

        public static TypeMappingEntry Get(this ITypeMapping typeMapping, JTokenType type, ILanguage to)
        {
            return typeMapping.Get(JsonLanguage.Instance, type.ToString(), to);
        }
    }
}