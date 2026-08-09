using System.Linq;
using System.Reflection;
using KY.Core.Extension;
using KY.Generator.Models;

namespace KY.Generator.Extensions
{
    public static class AssemblyExtension
    {
        public static SwitchableFramework GetSwitchableFramework(this Assembly assembly)
        {
            return assembly.GetTargetFramework().GetSwitchableFramework();
        }

        public static bool IsInBackground(this Assembly assembly)
        {
            return assembly.GetCustomAttributesData().Any(attribute =>
            {
                try
                {
                    return attribute?.AttributeType.Name == "GenerateInBackgroundAttribute";
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}