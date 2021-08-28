using KY.Generator.Csharp.Languages;
using KY.Generator.Mappings;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.OpenApi.Languages
{
    public static class TypeMapping
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Map<OpenApiLanguage>().To<CsharpLanguage>()
                       .From("array").To("List").Namespace("System.Collections.Generic").FromSystem()
                       .From("string").To("string").Nullable().FromSystem()
                       .From("boolean").To("bool").Nullable().FromSystem()
                       .From("integer").To("int").Nullable().FromSystem()
                       .From("number").To("double").Nullable().FromSystem();

            typeMapping.Map<OpenApiLanguage>().To<TypeScriptLanguage>()
                       .From("array").To("Array").FromSystem()
                       .From("string").To("string").Nullable().FromSystem()
                       .From("boolean").To("boolean").Nullable().FromSystem()
                       .From("integer").To("number").Nullable().FromSystem()
                       .From("number").To("number").Nullable().FromSystem();

            return typeMapping;
        }
    }
}
