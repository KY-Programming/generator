using System.Reflection;
using KY.Generator.Models;

namespace KY.Generator.Extensions;

public static class SwitchableFrameworkExtension
{
    public static string? FrameworkName(this SwitchableFramework framework)
    {
        FieldInfo memberInfos = framework.GetType().GetField(framework.ToString());
        return memberInfos?.GetCustomAttribute<FrameworkNameAttribute>()?.Name;
    }
}
