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
    public class FluentSyntax : IReadFluentSyntaxInternal, IWriteFluentSyntaxInternal, ISwitchToReadFluentSyntax
    {
        private readonly Options options;
        public IDependencyResolver Resolver { get; }

        public List<IExecutableSyntax> Syntaxes { get; } = new();

        public FluentSyntax(IDependencyResolver resolver, Options options)
        {
            this.options = options;
            this.Resolver = resolver;
        }

        public ISwitchToWriteFluentSyntax Read(Action<IReadFluentSyntax> action)
        {
            action(this);
            return this;
        }

        public void Write(Action<IWriteFluentSyntax> action)
        {
            action(this);
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

        public void FollowUp()
        {
            this.Syntaxes.ForEach(syntax => syntax.Commands.ForEach(command => command.FollowUp()));
        }

        ISwitchToReadFluentSyntax IFluentSyntax<ISwitchToReadFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        ISwitchToReadFluentSyntax IFluentSyntax<ISwitchToReadFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        ISwitchToReadFluentSyntax IFluentSyntax<ISwitchToReadFluentSyntax>.SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetMemberFluentSyntax> action) => this.SetMember(memberExpression, action);
        ISwitchToReadFluentSyntax IFluentSyntax<ISwitchToReadFluentSyntax>.SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetMemberFluentSyntax> action) => this.SetMember(memberExpression, action);
        ISwitchToReadFluentSyntax IFluentSyntax<ISwitchToReadFluentSyntax>.SetMember<T>(string name, Action<ISetMemberFluentSyntax> action) => this.SetMember<T>(name, action);
        ISwitchToWriteFluentSyntax IFluentSyntax<ISwitchToWriteFluentSyntax>.SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action) => this.SetGlobal(assembly, action);
        ISwitchToWriteFluentSyntax IFluentSyntax<ISwitchToWriteFluentSyntax>.SetType<T>(Action<ISetFluentSyntax> action) => this.SetType<T>(action);
        ISwitchToWriteFluentSyntax IFluentSyntax<ISwitchToWriteFluentSyntax>.SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetMemberFluentSyntax> action) => this.SetMember(memberExpression, action);
        ISwitchToWriteFluentSyntax IFluentSyntax<ISwitchToWriteFluentSyntax>.SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetMemberFluentSyntax> action) => this.SetMember(memberExpression, action);
        ISwitchToWriteFluentSyntax IFluentSyntax<ISwitchToWriteFluentSyntax>.SetMember<T>(string name, Action<ISetMemberFluentSyntax> action) => this.SetMember<T>(name, action);


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

        private FluentSyntax SetMember<T>(Expression<Func<T, object>> memberAction, Action<ISetMemberFluentSyntax> action)
        {
            action(new SetFluentMemberSyntax(memberAction.ExtractMember(), this.options));
            return this;
        }

        private FluentSyntax SetMember<T>(Expression<Action<T>> memberAction, Action<ISetMemberFluentSyntax> action)
        {
            action(new SetFluentMemberSyntax(memberAction.ExtractMember(), this.options));
            return this;
        }

        private FluentSyntax SetMember<T>(string name, Action<ISetMemberFluentSyntax> action)
        {
            Type type = typeof(T);
            type.GetMembers().Where(x => x.Name == name).ForEach(member => action(new SetFluentMemberSyntax(member, this.options)));
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

        public IWriteFluentSyntax ForceOverwrite()
        {
            this.options.Current.ForceOverwrite = true;
            return this;
        }

        public IWriteFluentSyntax FileName(Action<IFileNameFluentSyntax> action)
        {
            action(new FileNameFluentSyntax(this.options.Current));
            return this;
        }

        public IWriteFluentSyntax Formatter(string command)
        {
            this.options.Current.Formatter = command;
            return this;
        }
    }
}
