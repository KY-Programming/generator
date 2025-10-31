namespace KY.Generator;

public interface IGeneratorCommandAdditionalParameterAttribute
{
    IEnumerable<AttributeCommandConfiguration> Commands { get; }
}
