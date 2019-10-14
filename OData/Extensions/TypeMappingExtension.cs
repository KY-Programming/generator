using KY.Generator.Mappings;
using KY.Generator.OData.Language;
using KY.Generator.TypeScript.Languages;

namespace KY.Generator.OData.Extensions
{
    public static class TypeMappingExtension
    {
        public static ITypeMapping Initialize(this ITypeMapping typeMapping)
        {
            typeMapping.Map(ODataLanguage.Instance, "Edm.Duration").To(TypeScriptLanguage.Instance, "string", "String");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Binary").To(TypeScriptLanguage.Instance, "string", "String");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Boolean").To(TypeScriptLanguage.Instance, "boolean", "Boolean");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Byte").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Date").To(TypeScriptLanguage.Instance, "Date");
            typeMapping.Map(ODataLanguage.Instance, "Edm.DateTimeOffset").To(TypeScriptLanguage.Instance, "Date");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Decimal").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Double").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Guid").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Int16").To(TypeScriptLanguage.Instance, "string", "String");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Int32").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Int64").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.SByte").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Single").To(TypeScriptLanguage.Instance, "number", "Number");
            typeMapping.Map(ODataLanguage.Instance, "Edm.String").To(TypeScriptLanguage.Instance, "string", "String");
            typeMapping.Map(ODataLanguage.Instance, "Edm.TimeOfDay").To(TypeScriptLanguage.Instance, "string", "String");
            typeMapping.Map(ODataLanguage.Instance, "Edm.Void").To(TypeScriptLanguage.Instance, "void");
            return typeMapping;
        }
    }
}