namespace KY.Generator;

public interface ISetMemberFluentSyntax : ISetFluentSyntax<ISetMemberFluentSyntax>
{
    /// <summary>
    /// Renames a method or property
    /// </summary>
    ISetMemberFluentSyntax Rename(string name);

    /// <summary>
    /// Changes the return type
    /// </summary>
    ISetMemberReturnTypeFluentSyntax ReturnType(Type type);

    /// <summary>
    /// Changes the return type. Maybe you have to specify also an import e.g. .ReturnType(...).ImportFile(...)
    /// </summary>
    ISetMemberReturnTypeFluentSyntax ReturnType(string typeName);
}
