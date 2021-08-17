using System.Linq;
using KY.Generator.Angular.Commands;

namespace KY.Generator.Angular.Fluent
{
    internal class AngularServiceSyntax : IAngularServiceSyntax
    {
        private readonly AngularWriteSyntax syntax;
        private readonly AngularServiceCommand command;

        public AngularServiceSyntax(AngularWriteSyntax syntax, AngularServiceCommand command)
        {
            this.syntax = syntax;
            this.command = command;
            this.command.Parameters.RelativeModelPath = this.syntax.Commands.OfType<AngularModelCommand>().FirstOrDefault()?.Parameters.RelativePath;
        }

        public IAngularServiceSyntax SkipHeader()
        {
            this.command.Parameters.SkipHeader = true;
            return this;
        }

        public IAngularServiceSyntax FormatNames(bool value = true)
        {
            this.command.Parameters.FormatNames = value;
            return this;
        }

        public IAngularServiceSyntax OutputPath(string path)
        {
            this.command.Parameters.RelativePath = path;
            return this;
        }

        public IAngularServiceSyntax Name(string name)
        {
            this.command.Parameters.Name = name;
            return this;
        }
    }
}
