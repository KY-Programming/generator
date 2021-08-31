using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<IExecutableSyntax> Syntaxes { get; } = new();

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

        public IGeneratorCommandResult Run()
        {
            CommandRunner runner = this.Resolver.Create<CommandRunner>();
            foreach (IExecutableSyntax syntax in this.Syntaxes)
            {
                IGeneratorCommandResult commandResult = runner.Run(syntax.Commands);
                if (!commandResult.Success)
                {
                    return commandResult;
                }
            }
            return new SuccessResult();
        }

        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetFluentSyntax> action) => this.SetMember(memberExpression, action);
        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetFluentSyntax> action) => this.SetMember(memberExpression, action);
        IDoReadFluentSyntax IFluentSyntax<IDoReadFluentSyntax>.SetMember<T>(string name, Action<ISetFluentSyntax> action) => this.SetMember<T>(name, action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetFluentSyntax> action) => this.SetMember(memberExpression, action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetFluentSyntax> action) => this.SetMember(memberExpression, action);
        IReadFluentSyntax IFluentSyntax<IReadFluentSyntax>.SetMember<T>(string name, Action<ISetFluentSyntax> action) => this.SetMember<T>(name, action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetFluentSyntax> action) => this.SetMember(memberExpression, action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetFluentSyntax> action) => this.SetMember(memberExpression, action);
        IWriteFluentSyntax IFluentSyntax<IWriteFluentSyntax>.SetMember<T>(string name, Action<ISetFluentSyntax> action) => this.SetMember<T>(name, action);

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

        private FluentSyntax SetMember<T>(Expression<Func<T, object>> memberAction, Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(memberAction.ExtractMember(), this.options));
            return this;
        }

        private FluentSyntax SetMember<T>(Expression<Action<T>> memberAction, Action<ISetFluentSyntax> action)
        {
            action(new SetFluentSyntax(memberAction.ExtractMember(), this.options));
            return this;
        }

        private FluentSyntax SetMember<T>(string name, Action<ISetFluentSyntax> action)
        {
            Type type = typeof(T);
            type.GetMembers().Where(x => x.Name == name).ForEach(member => action(new SetFluentSyntax(member, this.options)));
            return this;
        }

        public IWriteFluentSyntax Formatting(Action<IFormattingFluentSyntax> action)
        {
            action(new FormattingFluentSyntax(this.options.Current.Formatting));
            return this;
        }

        public IWriteFluentSyntax NoHeader()
        {
            this.options.Current.AddHeader = false;
            return this;
        }

        public IWriteFluentSyntax FileName(Action<IFileNameFluentSyntax> action)
        {
            action(new FileNameFluentSyntax(this.options.Current));
            return this;
        }
    }
}
