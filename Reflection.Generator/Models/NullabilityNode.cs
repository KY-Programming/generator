using System.Reflection;

namespace KY.Generator.Reflection.Models;

/// <summary>
/// Target framework independent view on the nullable annotations of a member. Reflections <see cref="Type"/> instance
/// is identical for <c>string</c> and <c>string?</c>, the annotation only exists on the owning member. This node
/// mirrors the <see cref="Type"/> tree (element type of arrays, generic arguments) so the annotation can be carried
/// down while reading a type.
/// <para>
/// On target frameworks without <c>System.Reflection.NullabilityInfo</c> no node is created. The nullability of
/// nested types is unknown there and is treated as not annotated.
/// </para>
/// </summary>
public class NullabilityNode
{
    public bool IsNullable { get; }
    public NullabilityNode? ElementType { get; }
    public IReadOnlyList<NullabilityNode> GenericTypeArguments { get; }

    private NullabilityNode(bool isNullable, NullabilityNode? elementType, IReadOnlyList<NullabilityNode> genericTypeArguments)
    {
        this.IsNullable = isNullable;
        this.ElementType = elementType;
        this.GenericTypeArguments = genericTypeArguments;
    }

    /// <summary>
    /// Returns the node for the generic argument at the given index or null if the annotations are unknown
    /// </summary>
    public NullabilityNode? GetGenericTypeArgument(int index)
    {
        return index >= 0 && index < this.GenericTypeArguments.Count ? this.GenericTypeArguments[index] : null;
    }

#if NET6_0_OR_GREATER
    public static NullabilityNode Create(NullabilityInfo info)
    {
        return new NullabilityNode(info.ReadState == NullabilityState.Nullable,
                                   info.ElementType == null ? null : Create(info.ElementType),
                                   info.GenericTypeArguments.Select(Create).ToList());
    }
#endif
}
