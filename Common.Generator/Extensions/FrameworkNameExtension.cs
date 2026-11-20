using System.Runtime.Versioning;
using KY.Core.Extension;
using KY.Generator.Models;

namespace KY.Generator.Extensions;

public static class FrameworkNameExtension
{
    public static SwitchableFramework GetSwitchableFramework(this FrameworkName targetFramework)
    {
        if (targetFramework.IsFramework() && targetFramework.Version.Major <= 4)
        {
            return SwitchableFramework.Net4;
        }
        // There is no tool for anything older than .NET 8 (.NET Core 2/3, .NET 5-7) to switch to - such an assembly is
        // read by the process that is already running, a newer .NET that can load it
        if (targetFramework.IsCore() && targetFramework.Version.Major == 8)
        {
            return SwitchableFramework.Net8;
        }
        if (targetFramework.IsCore() && targetFramework.Version.Major == 9)
        {
            return SwitchableFramework.Net9;
        }
        if (targetFramework.IsCore() && targetFramework.Version.Major == 10)
        {
            return SwitchableFramework.Net10;
        }
        if (targetFramework.IsCore() && targetFramework.Version.Major == 11)
        {
            return SwitchableFramework.Net11;
        }
        return SwitchableFramework.None;
    }
}
