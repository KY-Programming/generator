using KY.Generator.Mappings;
using KY.Generator.OData.Language;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.OData.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Map<ODataLanguage>().To<TypeScriptLanguage>()
                       .From("Edm.Binary").To("string", "String")
                       .From("Edm.Boolean").To("boolean", "Boolean")
                       .From("Edm.Byte").To("number", "Number")
                       .From("Edm.Date").To("Date")
                       .From("Edm.DateTimeOffset").To("Date")
                       .From("Edm.Decimal").To("number", "Number")
                       .From("Edm.Double").To("number", "Number")
                       .From("Edm.Decimal").To("number", "Number")
                       .From("Edm.Duration").To("string", "String")
                       .From("Edm.Guid").To("string", "String")
                       .From("Edm.Int16").To("number", "Number")
                       .From("Edm.Int32").To("number", "Number")
                       .From("Edm.Int64").To("number", "Number")
                       .From("Edm.SByte").To("number", "Number")
                       .From("Edm.Single").To("number", "Number")
                       .From("Edm.String").To("string", "String")
                       .From("Edm.TimeOfDay").To("string", "String")
                       .From("Edm.Void").To("void");
            return typeMapping;
        }
    }
}
