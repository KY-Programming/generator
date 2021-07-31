using System.Linq;
using KY.Core;
using KY.Generator.Angular.Commands;

namespace KY.Generator.Angular.Fluent
{
    internal class AngularModelSyntax : IAngularModelSyntax
    {
        private readonly AngularWriteSyntax syntax;
        private readonly AngularModelCommand command;

        public AngularModelSyntax(AngularWriteSyntax syntax, AngularModelCommand command)
        {
            this.syntax = syntax;
            this.command = command;
        }

        public IAngularModelSyntax SkipHeader()
        {
            this.command.Parameters.SkipHeader = true;
            return this;
        }

        public IAngularModelSyntax FormatNames(bool value = true)
        {
            this.command.Parameters.FormatNames = value;
            return this;
        }

        public IAngularModelSyntax OutputPath(string path)
        {
            this.command.Parameters.RelativePath = path;
            this.syntax.Commands.OfType<AngularServiceCommand>().ForEach(x => x.Parameters.RelativeModelPath = path);
            return this;
        }

        public IAngularModelSyntax SkipNamespace(bool value = true)
        {
            this.command.Parameters.SkipNamespace = value;
            return this;
        }

        public IAngularModelSyntax PropertiesToFields(bool value = true)
        {
            this.command.Parameters.PropertiesToFields = value;
            return this;
        }

        public IAngularModelSyntax FieldsToProperties(bool value = true)
        {
            this.command.Parameters.FieldsToProperties = value;
            return this;
        }

        public IAngularModelSyntax Strict(bool value = true)
        {
            this.command.Parameters.Strict = value;
            return this;
        }
    }
}
