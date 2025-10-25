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
            typeMapping.Map<JsonLanguage>().To<CsharpLanguage>()
                       .From(JTokenType.Array.ToString()).To("List").Nullable().Namespace("System.Collections.Generic").FromSystem()
                       .From(JTokenType.Boolean.ToString()).To("bool").FromSystem()
                       .From(JTokenType.Bytes.ToString()).To("byte").FromSystem()
                       .From(JTokenType.Integer.ToString()).To("int").FromSystem()
                       .From(JTokenType.Float.ToString()).To("double").FromSystem()
                       .From(JTokenType.String.ToString()).To("string").Nullable().FromSystem()
                       .From(JTokenType.Date.ToString()).To("DateTime").Namespace("System").FromSystem()
                       .From(JTokenType.Guid.ToString()).To("Guid").Namespace("System").FromSystem()
                       .From(JTokenType.Uri.ToString()).To("Uri").Namespace("System").FromSystem()
                       .From(JTokenType.TimeSpan.ToString()).To("TimeSpan").Namespace("System").FromSystem()
                       .From(JTokenType.Object.ToString()).To("object").Nullable().FromSystem();

            typeMapping.Map<JsonLanguage>().To<TypeScriptLanguage>()
                       .From(JTokenType.Array.ToString()).To("Array").Nullable().Namespace("System.Collections.Generic").FromSystem()
                       .From(JTokenType.Boolean.ToString()).To("boolean").Nullable().FromSystem()
                       .From(JTokenType.Bytes.ToString()).To("number").Nullable().FromSystem()
                       .From(JTokenType.Integer.ToString()).To("number").Nullable().FromSystem()
                       .From(JTokenType.Float.ToString()).To("number").Nullable().FromSystem()
                       .From(JTokenType.String.ToString()).To("string").Nullable().FromSystem()
                       .From(JTokenType.Date.ToString()).To("Date").Nullable().FromSystem()
                       .From(JTokenType.Guid.ToString()).To("string").Nullable().FromSystem()
                       .From(JTokenType.Uri.ToString()).To("string").Nullable().FromSystem()
                       .From(JTokenType.TimeSpan.ToString()).To("string").Nullable().FromSystem()
                       .From(JTokenType.Object.ToString()).To("Object").Nullable().FromSystem();

            return typeMapping;
        }

        public static TypeMappingEntry Get(this ITypeMapping typeMapping, JTokenType type, ILanguage to)
        {
            return typeMapping.Get(JsonLanguage.Instance, type.ToString(), to);
        }
    }
}
