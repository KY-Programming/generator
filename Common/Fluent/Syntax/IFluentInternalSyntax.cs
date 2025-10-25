using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator.Syntax
{
    public interface IFluentInternalSyntax
    {
        IDependencyResolver Resolver { get; }
        IGeneratorCommandResult Run();
        void FollowUp();
        List<IExecutableSyntax> Syntaxes { get; }
    }
}
