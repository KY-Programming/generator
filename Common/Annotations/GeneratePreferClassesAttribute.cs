namespace KY.Generator;

/// <summary>
/// Generates classes instead of interfaces for the decorated type and for every type generated through it - the types
/// of its properties, fields and base types, or on a controller the models of its service. On an assembly it applies to
/// every type of it. The counterpart of <see cref="GeneratePreferInterfacesAttribute"/>, it wins over a
/// <c>preferInterfaces</c> from further out - a <c>ky-generator.json</c>, a command or the assembly.
/// </summary>
/// <remarks>
/// A type reached through two entries with different preferences follows the entry it was read through first.
/// To pin a single type regardless of any preference use <see cref="GenerateClassAttribute.ForceClass"/>.
/// A C# interface is still generated as an interface.
/// </remarks>
/// <example>
/// <code>
/// // ky-generator.json: { "options": { "preferInterfaces": true } }
///
/// [GeneratePreferClasses]
/// public class Settings      // generated as class Settings
/// {
///     public Detail Detail { get; set; }   // Detail is generated as class too
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = false)]
public class GeneratePreferClassesAttribute : Attribute
{
}
