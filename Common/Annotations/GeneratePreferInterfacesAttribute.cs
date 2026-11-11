namespace KY.Generator;

/// <summary>
/// Generates interfaces instead of classes for the decorated type and for every type generated through it - the types
/// of its properties, fields and base types, or on a controller the models of its service. On an assembly it applies
/// to every type of it. The counterpart of <see cref="GeneratePreferClassesAttribute"/>.
/// </summary>
/// <remarks>
/// A model with constants stays a class, an interface can not carry their values. A type reached through two entries
/// with different preferences follows the entry it was read through first. To pin a single type regardless of any
/// preference use <see cref="GenerateClassAttribute.ForceInterface"/>.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false)]
public class GeneratePreferInterfacesAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("angular-model", "-prefer-interfaces")
    ];
}
