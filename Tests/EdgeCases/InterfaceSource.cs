using KY.Generator;

namespace EdgeCases;

/// <summary>
/// An interface as the generation source. Everywhere else in the suite interfaces are only an output style
/// (GeneratePreferInterfaces); here the annotated C# type itself is an interface.
/// </summary>
[GenerateTypeScriptModel("Output/InterfaceSource")]
public interface IGeneratedInterface
{
    string Name { get; set; }
    int Value { get; }
    string? NullableName { get; set; }
    List<string> Items { get; set; }
    ISubInterface Sub { get; set; }
}

/// <summary>
/// Reached only through the annotated interface, so it has to be pulled in as a dependency.
/// </summary>
public interface ISubInterface
{
    string Name { get; set; }
}

/// <summary>
/// Interface inheritance - the derived interface has to carry or extend the base members.
/// </summary>
[GenerateTypeScriptModel("Output/InterfaceSource")]
public interface IDerivedInterface : IGeneratedInterface
{
    bool Extra { get; set; }
}

/// <summary>
/// A class implementing the annotated interface. It is generated independently and must not collide
/// with the interface file.
/// </summary>
[GenerateTypeScriptModel("Output/InterfaceSource"), GeneratePreferInterfaces]
public class ImplementingClass : IGeneratedInterface
{
    public string Name { get; set; } = "";
    public int Value { get; }
    public string? NullableName { get; set; }
    public List<string> Items { get; set; } = [];
    public ISubInterface Sub { get; set; } = null!;
}
