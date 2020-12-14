using KY.Generator.Angular.Commands;

namespace KY.Generator.Angular.Fluent
{
    public class AngularServiceSyntax : IAngularServiceOrAngularWriteSyntax
    {
        private readonly AngularWriteSyntax syntax;
        private readonly AngularServiceCommand command;

        public AngularServiceSyntax(AngularWriteSyntax syntax, AngularServiceCommand command)
        {
            this.syntax = syntax;
            this.command = command;
        }

        public IAngularServiceOrAngularWriteSyntax SkipHeader()
        {
            this.command.Parameters.SkipHeader = true;
            return this;
        }

        public IAngularServiceOrAngularWriteSyntax FormatNames(bool value = true)
        {
            this.command.Parameters.FormatNames = value;
            return this;
        }

        public IAngularServiceOrAngularWriteSyntax OutputPath(string path)
        {
            this.command.Parameters.RelativePath = path;
            return this;
        }
        
        public IAngularModelOrAngularWriteSyntax AngularModel()
        {
            return this.syntax.AngularModel();
        }

        public IAngularServiceOrAngularWriteSyntax AngularServices()
        {
            return this.syntax.AngularServices();
        }
    }
}