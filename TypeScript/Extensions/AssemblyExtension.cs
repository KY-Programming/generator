using System.Linq;
using System.Reflection;

namespace KY.Generator.TypeScript.Extensions
{
    public static class AssemblyExtension
    {
        public static bool IsStrict(this Assembly assembly)
        {
            return assembly.GetCustomAttributesData().Any(attribute =>
            {
                try
                {
                    return attribute?.AttributeType.Name == "GenerateStrictAttribute";
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}
