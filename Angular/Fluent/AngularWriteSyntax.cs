using KY.Generator.Angular.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Angular.Fluent
{
    internal class AngularWriteSyntax : IAngularWriteSyntax
    {
        public FluentSyntax Syntax { get; }

        public AngularWriteSyntax(FluentSyntax syntax)
        {
            this.Syntax = syntax;
        }

        public IAngularModelOrAngularWriteSyntax AngularModel()
        {
            AngularModelCommand command = new AngularModelCommand(this.Syntax.Resolver);
            this.Syntax.Commands.Add(command);
            return new AngularModelSyntax(this, command);
        }

        public IAngularServiceOrAngularWriteSyntax AngularServices()
        { 
            AngularServiceCommand command = new AngularServiceCommand(this.Syntax.Resolver);
            this.Syntax.Commands.Add(command);
            return new AngularServiceSyntax(this, command);
        }
    }
}