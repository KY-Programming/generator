namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false)]
public class GenerateStrictAttribute : Attribute
{
    public bool Strict { get; }

    public GenerateStrictAttribute(bool strict = true)
    {
        this.Strict = strict;
    }
}
