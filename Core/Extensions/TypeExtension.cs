using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;

namespace KY.Generator.Extensions
{
    public static class TypeExtension
    {
        public static Type IgnoreGeneric(this Type type, string nameSpace, string typeName)
        {
            return type.Namespace == nameSpace && type.Name.TrimEnd("`1") == typeName.TrimEnd("`1") ? type.GetGenericArguments().SingleOrDefault() ?? typeof(void) : type;
        }

        public static Type IgnoreGeneric(this Type type, params Type[] typesToIgnore)
        {
            bool isChanged = false;
            foreach (Type typeToIgnore in typesToIgnore)
            {
                Type oldType = type;
                type = type.IgnoreGeneric(typeToIgnore.Namespace, typeToIgnore.Name);
                isChanged |= oldType != type;
            }
            if (isChanged)
            {
                return type.IgnoreGeneric(typesToIgnore);
            }
            return type;
        }

        public static Type IgnoreGeneric(this Type type, IEnumerable<Type> typesToIgnore)
        {
            return type.IgnoreGeneric(typesToIgnore.ToArray());
        }
    }
}