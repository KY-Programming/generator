using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator.Syntax
{
    public class FluentSyntax : IReadFluentSyntaxInternal, IWriteFluentSyntaxInternal
    {
        private readonly Options options;
        public IDependencyResolver Resolver { get; }

        public List<IGeneratorCommand> Commands { get; set; } = new();

        public FluentSyntax(IDependencyResolver resolver, Options options)
        {
            this.options = options;
            this.Resolver = resolver;
        }

        public IWriteFluentSyntax Write()
        {
            return this;
        }

        public IWriteFluentSyntax SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(assembly, this.options));
            return this;
        }

        public IWriteFluentSyntax SetType<T>(Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(typeof(T), this.options));
            return this;
        }

        public IWriteFluentSyntax SetMember<T>(Expression<T> memberAction, Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(memberAction.ExtractMember(), this.options));
            return this;
        }
    }
}
