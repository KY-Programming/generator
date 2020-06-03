using System;
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
    }
}