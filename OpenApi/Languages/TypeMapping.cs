using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.OpenApi.Languages
{
    public static class TypeMapping
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Add(OpenApiLanguage.Instance, "array", CsharpLanguage.Instance, "List", nameSpace: "System.Collections.Generic", fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "string", CsharpLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "boolean", CsharpLanguage.Instance, "bool", true, fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "integer", CsharpLanguage.Instance, "int", true, fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "number", CsharpLanguage.Instance, "double", true, fromSystem: true);
            
            typeMapping.Add(OpenApiLanguage.Instance, "array", TypeScriptLanguage.Instance, "Array", fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "string", TypeScriptLanguage.Instance, "string", true, fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "boolean", TypeScriptLanguage.Instance, "boolean", true, fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "integer", TypeScriptLanguage.Instance, "number", true, fromSystem: true);
            typeMapping.Add(OpenApiLanguage.Instance, "number", TypeScriptLanguage.Instance, "number", true, fromSystem: true);

            return typeMapping;
        }
    }
}