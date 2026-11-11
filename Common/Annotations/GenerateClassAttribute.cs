namespace KY.Generator;

/// <summary>
/// Configures generation for a class, interface, struct or enum. Optionally renames it
/// (<see cref="GenerateNamedAttribute.Name"/>) or applies a substring replacement on its name
/// (<see cref="GenerateNamedAttribute.Replace"/> / <see cref="GenerateNamedAttribute.With"/>), and pins it to a class
/// (<see cref="ForceClass"/>) or an interface (<see cref="ForceInterface"/>).
/// </summary>
/// <remarks>
/// This is the class level counterpart of <see cref="GenerateMemberAttribute"/> and the successor of
/// the removed <c>GenerateRenameAttribute</c>: <c>[GenerateRename("Dto")]</c> becomes
/// <c>[GenerateClass(Replace = "Dto")]</c>. Type overrides are intentionally not available here,
/// a class has no type of its own to override. Like the naming, the force flags apply to the decorated type only and
/// are not passed on to the types generated through it.
/// </remarks>
/// <example>
/// <code>
/// [GenerateClass(Replace = "Dto")]
/// public class AccountDto { }        // generated as Account
///
/// [GenerateClass(Name = "Account")]
/// public class AccountTransfer { }   // generated as Account
///
/// [GenerateClass(ForceClass = true)]
/// public class Settings { }          // always generated as class, even with preferInterfaces
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false, AllowMultiple = true)]
public class GenerateClassAttribute : GenerateNamedAttribute
{
    /// <summary>
    /// Generates this type as a class, no matter which preference applies around it
    /// (<see cref="GeneratePreferInterfacesAttribute"/>, <c>preferInterfaces</c> of a <c>ky-generator.json</c> or a
    /// command). The generation fails on a C# interface, on an enum and together with <see cref="ForceInterface"/>
    /// </summary>
    public bool ForceClass { get; set; }

    /// <summary>
    /// Generates this type as an interface, no matter which preference applies around it
    /// (<see cref="GeneratePreferClassesAttribute"/>, a <c>ky-generator.json</c> or a command). The generation fails on
    /// a type with constants - an interface can not carry their values -, on an enum and together with
    /// <see cref="ForceClass"/>
    /// </summary>
    public bool ForceInterface { get; set; }
}
