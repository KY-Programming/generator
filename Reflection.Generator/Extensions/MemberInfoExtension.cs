using System.Reflection;

namespace KY.Generator.Reflection.Extensions;

public static class MemberInfoExtension
{
    private static readonly string[] requiredAttributeNames =
    [
        "System.ComponentModel.DataAnnotations.RequiredAttribute",
        "System.Runtime.CompilerServices.RequiredMemberAttribute"
    ];

    public static bool IsNullable(this FieldInfo fieldInfo)
    {
#if NET6_0_OR_GREATER
        NullabilityInfoContext context = new();
        NullabilityInfo nullability = context.Create(fieldInfo);
        return nullability.ReadState == NullabilityState.Nullable;
#else
        return true;
#endif
    }

    public static bool IsNullable(this PropertyInfo propertyInfo)
    {
#if NET6_0_OR_GREATER
        NullabilityInfoContext context = new();
        NullabilityInfo nullability = context.Create(propertyInfo);
        return nullability.ReadState == NullabilityState.Nullable;
#else
        return true;
#endif
    }

    public static bool IsNullable(this ParameterInfo parameterInfo)
    {
#if NET6_0_OR_GREATER
        NullabilityInfoContext context = new();
        NullabilityInfo nullability = context.Create(parameterInfo);
        return nullability.ReadState == NullabilityState.Nullable;
#else
        return true;
#endif
    }

    public static bool IsNullable(this EventInfo eventInfo)
    {
#if NET6_0_OR_GREATER
        NullabilityInfoContext context = new();
        NullabilityInfo nullability = context.Create(eventInfo);
        return nullability.ReadState == NullabilityState.Nullable;
#else
        return true;
#endif
    }

    public static bool IsRequired(this MemberInfo member)
    {
        return member.CustomAttributes.Any(x => requiredAttributeNames.Contains(x.AttributeType.FullName));
    }
}
