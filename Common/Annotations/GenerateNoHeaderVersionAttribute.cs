namespace KY.Generator;

/// <summary>
/// Writes the <code>&lt;auto-generated&gt;</code> header without the version of the generator, so an updated generator
/// alone does not change every generated file.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false)]
public class GenerateNoHeaderVersionAttribute : Attribute
{ }
