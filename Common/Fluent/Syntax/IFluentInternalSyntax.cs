using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator.Syntax;

public interface IFluentInternalSyntax
{
    IDependencyResolver Resolver { get; }
    IGeneratorCommandResult Run();
    void FollowUp();
    List<ExecutableSyntax> Syntaxes { get; }
}
