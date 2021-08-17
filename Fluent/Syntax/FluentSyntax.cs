using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;

namespace KY.Generator.Syntax
{
    public class FluentSyntax : IReadFluentSyntaxInternal, IWriteFluentSyntaxInternal, IDoReadFluentSyntax
    {
        private readonly Options options;
        public IDependencyResolver Resolver { get; }

        public List<IGeneratorCommand> Commands { get; set; } = new();

        public FluentSyntax(IDependencyResolver resolver, Options options)
        {
            this.options = options;
            this.Resolver = resolver;
        }

        public IReadFluentSyntax Read()
        {
            return this;
        }

        public IWriteFluentSyntax Write()
        {
            return this;
        }

        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetMember<T>(Expression<T> memberAction, Action<ISetFluentSyntax> action) => this.SetMember(memberAction, action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetMember<T>(Expression<T> memberAction, Action<ISetFluentSyntax> action) => this.SetMember(memberAction, action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetMember<T>(Expression<T> memberAction, Action<ISetFluentSyntax> action) => this.SetMember(memberAction, action);

        private FluentSyntax SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(assembly, this.options));
            return this;
        }

        private FluentSyntax SetType<T>(Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(typeof(T), this.options));
            return this;
        }

        private FluentSyntax SetMember<T>(Expression<T> memberAction, Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(memberAction.ExtractMember(), this.options));
            return this;
        }
    }
}
