using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.OData.MetaData;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.OData.Extensions
{
    public static class ODataMetaDataPropertyExtension
    {
        public static TypeMappingEntry GetMapping(this ODataMetaDataProperty property, ClassTemplate classTemplate, ILanguage language)
        {
            TypeMappingEntry mapping = StaticResolver.TypeMapping.Get(Code.Language, property.Type, language);
            if (mapping.Namespace != null)
            {
                classTemplate.AddUsing(mapping.Namespace, null, null);
            }
            return mapping;
        }

        public static TypeTemplate GetMappedType(this ODataMetaDataProperty property, ClassTemplate classTemplate, ILanguage language)
        {
            return KY.Generator.Code.Type(property.GetMapping(classTemplate, language).ToType);
        }

        public static string GetConstructor(this ODataMetaDataProperty property, ClassTemplate classTemplate, ILanguage language)
        {
            TypeMappingEntry mapping = property.GetMapping(classTemplate, language);
            return mapping.Constructor;
        }
    }
}