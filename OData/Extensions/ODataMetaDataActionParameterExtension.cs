using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.OData.MetaData;
using KY.Generator.Templates;
using KY.Generator.Templates.Extensions;

namespace KY.Generator.OData.Extensions
{
    public static class ODataMetaDataActionParameterExtension
    {
        public static TypeMappingEntry GetMapping(this ODataMetaDataActionParameter parameter, ClassTemplate classTemplate, ILanguage language)
        {
            TypeMappingEntry mapping = StaticResolver.TypeMapping.Get(Code.Language, parameter.Type, language);
            if (mapping.Namespace != null)
            {
                classTemplate.AddUsing(mapping.Namespace, null, null);
            }
            return mapping;
        }

        public static string GetConstructor(this ODataMetaDataActionParameter parameter, ClassTemplate classTemplate, ILanguage language)
        {
            TypeMappingEntry mapping = parameter.GetMapping(classTemplate, language);
            return mapping.Constructor;
        }
    }
}