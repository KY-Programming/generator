namespace KY.Generator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
public class GenerateOnlySubTypesAttribute : Attribute
{ }
