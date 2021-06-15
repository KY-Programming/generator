using KY.Generator.Reflection.Commands;
using KY.Generator.Syntax;

namespace KY.Generator.Reflection.Fluent
{
    internal class ReflectionWriteSyntax : IReflectionWriteSyntax
    {
        private readonly IWriteFluentSyntaxInternal syntax;
        private ReflectionWriteCommand command;

        public ReflectionWriteSyntax(IWriteFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IReflectionWriteSyntax Models(string relativePath)
        {
            this.command = new ReflectionWriteCommand(this.syntax.Resolver);
            this.command.Parameters.RelativePath = relativePath;
            this.syntax.Commands.Add(this.command);
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