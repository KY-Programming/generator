using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator.Syntax
{
    public class FluentSyntax : IReadFluentSyntaxInternal, IWriteFluentSyntaxInternal
    {
        public IDependencyResolver Resolver { get; }

        public List<IGeneratorCommand> Commands { get; set; } = new List<IGeneratorCommand>();

        public FluentSyntax(IDependencyResolver resolver)
        {
            this.Resolver = resolver;
        }

        public IWriteFluentSyntax Write()
        {
            return this;
        }

        public IReadFluentSyntax SetType<T>(Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(typeof(T)));
            return this;
        }
    }
}
