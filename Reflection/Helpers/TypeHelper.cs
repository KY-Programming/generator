using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KY.Core;

namespace KY.Generator.Reflection.Helpers
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
    }
}
