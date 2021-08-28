using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Reflection.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent
{
    internal class ReflectionWriteSyntax : IReflectionWriteSyntax, IExecutableSyntax
    {
        private readonly IWriteFluentSyntaxInternal syntax;
        private ReflectionWriteCommand command;

        public List<IGeneratorCommand> Commands { get; } = new();

        public ReflectionWriteSyntax(IWriteFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IReflectionWriteSyntax Models(string relativePath)
        {
            this.command = this.syntax.Resolver.Create<ReflectionWriteCommand>();
            this.command.Parameters.RelativePath = relativePath;
            this.Commands.Add(this.command);
            return this;
        }

        public IReflectionWriteSyntax PropertiesToFields()
        {
            this.command.Parameters.PropertiesToFields = true;
            return this;
        }

        public IReflectionWriteSyntax FieldsToProperties()
        {
            this.command.Parameters.FieldsToProperties = true;
            return this;
        }
    }
}
