using System.Reflection;
using KY.Generator.Reflection.Models;

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

    /// <summary>
    /// Reads the complete nullable annotation tree of the property. Returns null if the target framework does not
    /// support reading nullable annotations
    /// </summary>
    public static NullabilityNode? GetNullability(this PropertyInfo propertyInfo)
    {
#if NET6_0_OR_GREATER
        return NullabilityNode.Create(new NullabilityInfoContext().Create(propertyInfo));
#else
        return null;
#endif
    }

    /// <summary>
    /// Reads the complete nullable annotation tree of the field. Returns null if the target framework does not
    /// support reading nullable annotations
    /// </summary>
    public static NullabilityNode? GetNullability(this FieldInfo fieldInfo)
    {
#if NET6_0_OR_GREATER
        return NullabilityNode.Create(new NullabilityInfoContext().Create(fieldInfo));
#else
        return null;
#endif
    }

    public static bool IsRequired(this MemberInfo member)
    {
        return member.CustomAttributes.Any(x => requiredAttributeNames.Contains(x.AttributeType.FullName));
    }
}
