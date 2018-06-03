using KY.Generator.Mappings;

namespace KY.Generator.OData.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Map(Code.Language, "Edm.Duration").To(TypeScript.Code.Language, "string", "String");
            typeMapping.Map(Code.Language, "Edm.Binary").To(TypeScript.Code.Language, "string", "String");
            typeMapping.Map(Code.Language, "Edm.Boolean").To(TypeScript.Code.Language, "boolean", "Boolean");
            typeMapping.Map(Code.Language, "Edm.Byte").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.Date").To(TypeScript.Code.Language, "Date");
            typeMapping.Map(Code.Language, "Edm.DateTimeOffset").To(TypeScript.Code.Language, "Date");
            typeMapping.Map(Code.Language, "Edm.Decimal").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.Double").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.Guid").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.Int16").To(TypeScript.Code.Language, "string", "String");
            typeMapping.Map(Code.Language, "Edm.Int32").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.Int64").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.SByte").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.Single").To(TypeScript.Code.Language, "number", "Number");
            typeMapping.Map(Code.Language, "Edm.String").To(TypeScript.Code.Language, "string", "String");
            typeMapping.Map(Code.Language, "Edm.TimeOfDay").To(TypeScript.Code.Language, "string", "String");
            return typeMapping;
        }
    }
}