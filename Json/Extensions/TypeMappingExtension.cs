using KY.Generator.Languages;
using KY.Generator.Mappings;
using Newtonsoft.Json.Linq;

namespace KY.Generator.Json.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(Code.Languages.Json(), JTokenType.Boolean.ToString(), Csharp.Code.Language, "bool");
            typeMapping.Add(Code.Languages.Json(), JTokenType.Bytes.ToString(), Csharp.Code.Language, "byte");
            typeMapping.Add(Code.Languages.Json(), JTokenType.Integer.ToString(), Csharp.Code.Language, "int");
            typeMapping.Add(Code.Languages.Json(), JTokenType.Float.ToString(), Csharp.Code.Language, "double");
            typeMapping.Add(Code.Languages.Json(), JTokenType.String.ToString(), Csharp.Code.Language, "string", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.Date.ToString(), Csharp.Code.Language, "DateTime", false, "System");
            typeMapping.Add(Code.Languages.Json(), JTokenType.Guid.ToString(), Csharp.Code.Language, "Guid", false, "System");
            typeMapping.Add(Code.Languages.Json(), JTokenType.Uri.ToString(), Csharp.Code.Language, "Uri", false, "Uri");
            typeMapping.Add(Code.Languages.Json(), JTokenType.TimeSpan.ToString(), Csharp.Code.Language, "TimeSpan", false, "TimeSpan");

            typeMapping.Add(Code.Languages.Json(), JTokenType.Boolean.ToString(), TypeScript.Code.Language, "boolean", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.Bytes.ToString(), TypeScript.Code.Language, "number", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.Integer.ToString(), TypeScript.Code.Language, "number", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.Float.ToString(), TypeScript.Code.Language, "number", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.String.ToString(), TypeScript.Code.Language, "string", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.Date.ToString(), TypeScript.Code.Language, "Date", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.Guid.ToString(), TypeScript.Code.Language, "string", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.Uri.ToString(), TypeScript.Code.Language, "string", true);
            typeMapping.Add(Code.Languages.Json(), JTokenType.TimeSpan.ToString(), TypeScript.Code.Language, "number", true);
            return typeMapping;
        }

        public static TypeMappingEntry Get(this ITypeMapping typeMapping, JTokenType type, ILanguage to)
        {
            return typeMapping.Get(Code.Languages.Json(), type.ToString(), to);
        }
    }
}