using System.Runtime.Versioning;
using KY.Core.Extension;
using KY.Generator.Models;

namespace KY.Generator.Extensions
{
    public static class FrameworkNameExtension
    {
        public static SwitchableFramework GetSwitchableFramework(this FrameworkName targetFramework)
        {
            if (targetFramework.IsFramework() && targetFramework.Version.Major <= 4)
            {
                return SwitchableFramework.Net4;
            }
            if (targetFramework.IsCore() && targetFramework.Version.Major <= 2)
            {
                return SwitchableFramework.NetCore2;
            }
            if (targetFramework.IsCore() && targetFramework.Version.Major == 3)
            {
                return SwitchableFramework.NetCore3;
            }
            if (targetFramework.IsCore() && targetFramework.Version.Major == 5)
            {
                return SwitchableFramework.Net5;
            }
            if (targetFramework.IsCore() && targetFramework.Version.Major == 6)
            {
                return SwitchableFramework.Net6;
            }
            if (targetFramework.IsCore() && targetFramework.Version.Major == 7)
            {
                return SwitchableFramework.Net7;
            }
            return SwitchableFramework.None;
        }
    }
}
