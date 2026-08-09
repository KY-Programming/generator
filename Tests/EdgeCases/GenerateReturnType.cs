using KY.Generator;

namespace EdgeCases;

[GenerateAngularModel("Output")]
public class GenerateReturnType
{
    [GenerateProperty(Type = typeof(string))]
    public MyCustomStringType ChangeTypeToString { get; set; }
    
}

public class MyCustomStringType
{
    
}
