using System.Reflection;

namespace KY.Generator.Reflection.Extensions;

public static class MemberInfoExtension
{
    public static bool IsNullable(this MemberInfo member)
    {
        return member.CustomAttributes.Any(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
    }

    public static bool IsRequired(this MemberInfo member)
    {
        return member.CustomAttributes.Any(x => x.AttributeType.FullName == "System.ComponentModel.DataAnnotations.RequiredAttribute");
    }
}
