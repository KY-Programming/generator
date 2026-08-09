using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;

namespace KY.Generator.Helpers
{
    internal static class TypeHelper
    {
        public static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                Logger.Error(exception);
                exception.LoaderExceptions.ForEach(Logger.Error);
                return Enumerable.Empty<Type>();
            }
        }

        /// <summary>
        /// The name a type is written with in the source code: a nested type is prefixed with its declaring
        /// types, e.g. 'Outer.Inner'. <see cref="Type.Name"/> alone would return 'Inner', which is not enough
        /// to find the type back, and <see cref="Type.FullName"/> would return the CLR form 'Namespace.Outer+Inner'.
        /// </summary>
        public static string GetSourceName(Type type)
        {
            string name = type.Name;
            for (Type declaringType = type.DeclaringType; declaringType != null; declaringType = declaringType.DeclaringType)
            {
                name = $"{declaringType.Name}.{name}";
            }
            return name;
        }
    }
}
