namespace KY.Generator;

/// <summary>
/// Configures generation for a field. Optionally renames it (<see cref="GenerateMemberAttribute.Name"/>),
/// overrides its generated type (<see cref="GenerateMemberAttribute.Type"/>), or applies a substring
/// replacement on its name (<see cref="GenerateMemberAttribute.Replace"/> / <see cref="GenerateMemberAttribute.With"/>).
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class GenerateFieldAttribute : GenerateMemberAttribute
{
}
