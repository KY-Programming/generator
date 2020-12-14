using KY.Generator.Angular.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Angular.Fluent
{
    public class AngularWriteSyntax : IAngularWriteSyntax
    {
        private readonly FluentSyntax syntax;

        public AngularWriteSyntax(FluentSyntax syntax)
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