using KY.Generator.Angular.Commands;

namespace KY.Generator.Angular.Fluent
{
    public class AngularModelSyntax : IAngularModelOrAngularWriteSyntax
    {
        private readonly AngularWriteSyntax syntax;
        private readonly AngularModelCommand command;

        public AngularModelSyntax(AngularWriteSyntax syntax, AngularModelCommand command)
        {
            this.syntax = syntax;
            this.command = command;
        }

        public IAngularModelOrAngularWriteSyntax SkipHeader()
        {
            this.command.Parameters.SkipHeader = true;
            return this;
        }

        public IAngularModelOrAngularWriteSyntax FormatNames(bool value = true)
        {
            this.command.Parameters.FormatNames = value;
            return this;
        }

        public IAngularModelOrAngularWriteSyntax OutputPath(string path)
        {
            this.command.Parameters.RelativePath = path;
            return this;
        }

        public IAngularModelOrAngularWriteSyntax SkipNamespace(bool value = true)
        {
            this.command.Parameters.SkipNamespace = value;
            return this;
        }

        public IAngularModelOrAngularWriteSyntax PropertiesToFields(bool value = true)
        {
            this.command.Parameters.PropertiesToFields = value;
            return this;
        }

        public IAngularModelOrAngularWriteSyntax FieldsToProperties(bool value = true)
        {
            this.command.Parameters.FieldsToProperties = value;
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