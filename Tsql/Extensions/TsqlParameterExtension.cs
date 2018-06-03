using KY.Generator.Csharp.Extensions;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Tsql.Type;

namespace KY.Generator.Tsql.Extensions
{
    public static class TsqlParameterExtension
    {
        public static TypeTemplate GetMappedType(this TsqlParameter parameter, ClassTemplate classTemplate, ILanguage language)
        {
            TypeMappingEntry mapping = StaticResolver.TypeMapping.Get(Code.Language, parameter.Type, language);
            if (mapping.Namespace != null)
            {
                classTemplate.AddUsing(mapping.Namespace);
            }
            return KY.Generator.Code.Type(mapping.ToType, parameter.IsNullable && mapping.Nullable);
        }
    }
}