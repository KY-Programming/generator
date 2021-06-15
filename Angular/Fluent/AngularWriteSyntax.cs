using System;
using System.Collections.Generic;
using KY.Generator.Angular.Commands;
using KY.Generator.Command;
using KY.Generator.Syntax;

namespace KY.Generator.Angular.Fluent
{
    internal class AngularWriteSyntax : IAngularWriteSyntax
    {
        private readonly IWriteFluentSyntaxInternal syntax;

        public List<IGeneratorCommand> Commands => this.syntax.Commands;

        public AngularWriteSyntax(IWriteFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IAngularWriteSyntax Models(Action<IAngularModelSyntax> action = null)
        {
            AngularModelCommand command = new AngularModelCommand(this.syntax.Resolver);
            this.syntax.Commands.Add(command);
            action?.Invoke(new AngularModelSyntax(this, command));
            return this;
        }

        public IAngularWriteSyntax Services(Action<IAngularServiceSyntax> action = null)
        { 
            AngularServiceCommand command = new AngularServiceCommand(this.syntax.Resolver);
            this.syntax.Commands.Add(command);
            action?.Invoke(new AngularServiceSyntax(this, command));
            return this;
        }
    }
}