using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator.Syntax
{
    public class FluentSyntax : IReadFluentSyntax, IWriteFluentSyntax
    {
        public IDependencyResolver Resolver { get; }
        public List<IGeneratorCommand> Commands { get; } = new List<IGeneratorCommand>();

        public FluentSyntax(IDependencyResolver resolver)
        {
            this.Resolver = resolver;
        }
    }
}