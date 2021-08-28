using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Json.Commands;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public class JsonWriteSyntax : IJsonWriteSyntax, IJsonWriteModelSyntax, IJsonWriteModelOrReaderSyntax, IExecutableSyntax
    {
        private JsonWriteCommand command;
        private readonly IWriteFluentSyntaxInternal syntax;

        public List<IGeneratorCommand> Commands { get; } = new();

        public JsonWriteSyntax(IWriteFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IJsonWriteModelOrReaderSyntax Model(string relativePath, string name, string nameSpace)
        {
            this.command = this.syntax.Resolver.Create<JsonWriteCommand>();
            this.command.Parameters.RelativePath = relativePath;
            this.command.Parameters.ModelName = name;
            this.command.Parameters.ModelNamespace = nameSpace;
            this.Commands.Add(this.command);
            return this;
        }

        public IJsonWriteModelOrReaderSyntax FieldsToProperties()
        {
            this.command.Parameters.FieldsToProperties = true;
            return this;
        }

        public IJsonWriteModelOrReaderSyntax PropertiesToFields()
        {
            this.command.Parameters.PropertiesToFields = true;
            return this;
        }

        public IJsonWriteModelSyntax WithoutReader()
        {
            this.command.Parameters.WithReader = false;
            return this;
        }
    }
}
