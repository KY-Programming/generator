namespace KY.Generator;

/// <summary>
/// Generates code that is not restricted by TypeScripts strict mode. Without this attribute the generated code is
/// strict: members that can not be undefined get a default value and nullable members are written as a union with
/// <c>undefined</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false)]
public class GenerateNonStrictAttribute : Attribute
{
    public bool NonStrict { get; }

    /// <param name="nonStrict">Set to false to switch back to strict, e.g. for a single class in an assembly that is marked as non strict</param>
    public GenerateNonStrictAttribute(bool nonStrict = true)
    {
        this.NonStrict = nonStrict;
    }
}
