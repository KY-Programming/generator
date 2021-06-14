using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator.Syntax
{
    public interface IFluentSyntax
    {
        IDependencyResolver Resolver { get; }
        List<IGeneratorCommand> Commands { get; }
    }
}