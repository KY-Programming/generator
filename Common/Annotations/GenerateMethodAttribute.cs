namespace KY.Generator;

/// <summary>
/// Configures generation for a method. Optionally renames it (<see cref="GenerateMemberAttribute.Name"/>),
/// overrides its return type (<see cref="GenerateMemberAttribute.Type"/>), or applies a substring
/// replacement on its name (<see cref="GenerateMemberAttribute.Replace"/> / <see cref="GenerateMemberAttribute.With"/>).
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class GenerateMethodAttribute : GenerateMemberAttribute
{
}
