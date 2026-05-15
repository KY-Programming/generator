namespace KY.Generator;

/// <summary>
/// Configures generation for a property. Optionally renames it (<see cref="GenerateMemberAttribute.Name"/>),
/// overrides its generated type (<see cref="GenerateMemberAttribute.Type"/>), or applies a substring
/// replacement on its name (<see cref="GenerateMemberAttribute.Replace"/> / <see cref="GenerateMemberAttribute.With"/>).
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class GeneratePropertyAttribute : GenerateMemberAttribute
{
}
