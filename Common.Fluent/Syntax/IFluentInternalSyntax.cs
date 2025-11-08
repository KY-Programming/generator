using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator;

public interface IFluentInternalSyntax
{
    IDependencyResolver Resolver { get; }
    Task<IGeneratorCommandResult> Run();
    void FollowUp();
    List<IExecutableSyntax> Syntaxes { get; }
}
