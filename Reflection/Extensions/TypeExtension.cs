using System;
using System.Linq;
using KY.Generator.Csharp.Extensions;
using KY.Generator.Languages;
using KY.Generator.Mappings;
using KY.Generator.Templates;

namespace KY.Generator.Reflection.Extensions
{
    internal static class TypeExtension
    {
        public static TypeTemplate GetMappedType(this Type type, ClassTemplate classTemplate, ILanguage language)
        {
            if (type.IsGenericType)
            {
                TypeTemplate[] arguments = type.GenericTypeArguments.Select(x => Code.Type(TryGetMappedType(x, language))).ToArray();
                return Code.Generic(Cleanup(TryGetMappedType(type.GetGenericTypeDefinition(), language)), arguments);
            }
            TypeMappingEntry mapping = StaticResolver.TypeMapping.TryGet(Reflection.Language, type.FullName, language);
            if (mapping?.Namespace != null)
            {
                classTemplate.AddUsing(mapping.Namespace);
            }
            return Code.Type(mapping == null ? type.Name : mapping.ToType);
        }

        private static string TryGetMappedType(Type type, ILanguage language)
        {
            TypeMappingEntry mapping = StaticResolver.TypeMapping.TryGet(Reflection.Language, type.FullName, language);
            return mapping == null ? type.Name : mapping.ToType;
        }

        private static string Cleanup(string name)
        {
            int genericIndex = name.IndexOf('`');
            return genericIndex > 0 ? name.Substring(0, genericIndex) : name;
        }
    }
}