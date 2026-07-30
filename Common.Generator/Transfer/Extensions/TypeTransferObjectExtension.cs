using System;
using System.Linq;
using KY.Generator.Templates;

namespace KY.Generator.Transfer.Extensions
{
    public static class TypeTransferObjectExtension
    {
        public static TypeTemplate ToTemplate(this TypeTransferObject type)
        {
            if (type == null)
            {
                throw new NullReferenceException();
            }
            if (type.Generics.Count == 0)
            {
                return new LinkedTypeTemplate(type);
            }
            LinkedGenericTypeTemplate genericTypeTemplate = new(type);
            type.Generics.ForEach(g => genericTypeTemplate.Types.Add(g.ToTemplate()));
            return genericTypeTemplate;
        }

        private static TypeTemplate ToTemplate(this GenericAliasTransferObject generic)
        {
            TypeTemplate template = (generic.Type ?? generic.Alias).ToTemplate();
            return generic.IsNullable ? new NullableTypeTemplate(template) : template;
        }
    }
}
