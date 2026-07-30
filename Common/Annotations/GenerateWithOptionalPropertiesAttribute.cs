namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
public class GenerateWithOptionalPropertiesAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public IEnumerable<AttributeCommandConfiguration> Commands
    {
        get
        {
            return new[]
            {
                new AttributeCommandConfiguration("angular-model", "-with-optional-properties")
            };
        }
    }
}
