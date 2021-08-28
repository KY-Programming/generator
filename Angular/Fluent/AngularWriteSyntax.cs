using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Commands;
using KY.Generator.Command;
using KY.Generator.Syntax;

namespace KY.Generator.Angular.Fluent
{
    internal class AngularWriteSyntax : IAngularWriteSyntax
    {
        private readonly IDependencyResolver resolver;

        public List<IGeneratorCommand> Commands { get; } = new();

        public AngularWriteSyntax(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public IAngularWriteSyntax Models(Action<IAngularModelSyntax> action = null)
        {
            AngularModelCommand command = this.resolver.Create<AngularModelCommand>();
            this.Commands.Add(command);
            action?.Invoke(new AngularModelSyntax(this, command));
            return this;
        }

        public IAngularWriteSyntax Services(Action<IAngularServiceSyntax> action = null)
        {
            AngularServiceCommand command = this.resolver.Create<AngularServiceCommand>();
            this.Commands.Add(command);
            action?.Invoke(new AngularServiceSyntax(this, command));
            return this;
        }
    }
}
