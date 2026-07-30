namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, Inherited = false)]
public class GeneratePreferInterfacesAttribute : Attribute, IGeneratorCommandAdditionalParameterAttribute
{
    public IEnumerable<AttributeCommandConfiguration> Commands =>
    [
        new("angular-model", "-prefer-interfaces")
    ];
}
