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
    }
}