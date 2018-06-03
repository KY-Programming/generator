using KY.Generator.Csharp.Extensions;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Tsql.Type;

namespace KY.Generator.Tsql.Extensions
{
    public static class TsqlColumnExtension
    {
        public static TypeTemplate GetMappedType(this TsqlColumn column, ClassTemplate classTemplate, ILanguage language)
        {
            TypeMappingEntry mapping = StaticResolver.TypeMapping.Get(Code.Language, column.Type, language);
            if (mapping.Namespace != null)
            {
                classTemplate.AddUsing(mapping.Namespace);
            }
            return KY.Generator.Code.Type(mapping.ToType, column.IsNullable && mapping.Nullable);
        }
    }
}