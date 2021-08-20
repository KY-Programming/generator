using System;
using System.Linq;
using KY.Core;
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
                return new TypeTemplate(type.Name, type.Namespace, fromSystem: type.FromSystem, isNullable: type.IsNullable);
            }
            GenericTypeTemplate genericTypeTemplate = new GenericTypeTemplate(type.Name, type.Namespace, fromSystem: type.FromSystem, isNullable: type.IsNullable);
            type.Generics.Where(x => x.Type != null).ForEach(g => genericTypeTemplate.Types.Add(g.Type.ToTemplate()));
            return genericTypeTemplate;
        }
    }
}
