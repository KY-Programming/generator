namespace KY.Generator;

/// <summary>
/// Configures generation for a class, interface, struct or enum. Optionally renames it
/// (<see cref="GenerateNamedAttribute.Name"/>) or applies a substring replacement on its name
/// (<see cref="GenerateNamedAttribute.Replace"/> / <see cref="GenerateNamedAttribute.With"/>).
/// </summary>
/// <remarks>
/// This is the class level counterpart of <see cref="GenerateMemberAttribute"/> and the successor of
/// the removed <c>GenerateRenameAttribute</c>: <c>[GenerateRename("Dto")]</c> becomes
/// <c>[GenerateClass(Replace = "Dto")]</c>. Type overrides are intentionally not available here,
/// a class has no type of its own to override.
/// </remarks>
/// <example>
/// <code>
/// [GenerateClass(Replace = "Dto")]
/// public class AccountDto { }        // generated as Account
///
/// [GenerateClass(Name = "Account")]
/// public class AccountTransfer { }   // generated as Account
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false, AllowMultiple = true)]
public class GenerateClassAttribute : GenerateNamedAttribute
{
}
