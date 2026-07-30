namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public class GenerateAsyncAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public IEnumerable<AttributeCommandConfiguration> Commands { get; } = new[]
    {
        new AttributeCommandConfiguration("*", "-async")
    };
}
