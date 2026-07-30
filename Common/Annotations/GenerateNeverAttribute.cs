namespace KY.Generator;

/// <summary>
/// Marks a type that must never be generated. If the generator tries to write the decorated type, the generation is
/// aborted with an error that contains the file the type would have been written to.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
public class GenerateNeverAttribute : Attribute
{ }
