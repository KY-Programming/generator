namespace KY.Generator.Syntax;

public interface ISetMemberReturnTypeFluentSyntax : ISetMemberFluentSyntax
{
    /// <summary>
    /// Import a namespace
    /// </summary>
    ISetMemberFluentSyntax ImportNamespace(string nameSpace);
    
    /// <summary>
    /// Import a file by its name. Specify optionally a different import type name
    /// </summary>
    ISetMemberFluentSyntax ImportFile(string fileName, string type = null);
}
