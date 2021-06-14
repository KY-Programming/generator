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

        public IAngularModelOrAngularWriteSyntax AngularModel()
        {
            AngularModelCommand command = new AngularModelCommand(this.syntax.Resolver);
            this.syntax.Commands.Add(command);
            return new AngularModelSyntax(this, command);
        }

        public IAngularServiceOrAngularWriteSyntax AngularServices()
        { 
            AngularServiceCommand command = new AngularServiceCommand(this.syntax.Resolver);
            this.syntax.Commands.Add(command);
            return new AngularServiceSyntax(this, command);
        }
    }
}