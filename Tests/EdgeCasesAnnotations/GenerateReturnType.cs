using KY.Generator;

namespace EdgeCasesAnnotations;

[GenerateAngularModel("Output")]
public class GenerateReturnType
{
    [GenerateProperty(Type = typeof(string))]
    public MyCustomStringType ChangeTypeToString { get; set; }
    
}

public class MyCustomStringType
{
    
}
