namespace KY.Generator;

/// <summary>
/// Moves the generation of the annotated type - or of the whole assembly - to a process that outlives the build.
/// The build does not wait for it, so the files appear a moment after it finished.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public class GenerateInBackgroundAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public IEnumerable<AttributeCommandConfiguration> Commands { get; } = new[]
    {
        new AttributeCommandConfiguration("*", "-in-background")
    };
}
