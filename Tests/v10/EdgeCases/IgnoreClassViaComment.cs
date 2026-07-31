using KY.Generator;

namespace EdgeCases;

/// <summary>
/// This is a normal class.
/// Generator should not ignore this class
/// </summary>
[GenerateAngularModel("Output")]
public class IgnoreClassViaComment : IIgnoreMe
{
}

/// <summary>
/// Generator ignores this interface
/// </summary>
public interface IIgnoreMe
{
    
}
