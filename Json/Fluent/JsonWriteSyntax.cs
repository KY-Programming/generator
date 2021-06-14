using KY.Generator.Json.Commands;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public class JsonWriteSyntax : IJsonWriteSyntax, IJsonWriteModelSyntax, IJsonWriteModelOrReaderSyntax
    {
        private JsonWriteCommand command;
        private readonly IWriteFluentSyntaxInternal syntax;

        public JsonWriteSyntax(IWriteFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public IJsonWriteModelOrReaderSyntax Model(string relativePath, string name, string nameSpace)
        {
            this.command = new JsonWriteCommand(this.syntax.Resolver);
            this.command.Parameters.ModelPath = relativePath;
            this.command.Parameters.ModelName = name;
            this.command.Parameters.ModelNamespace = nameSpace;
            this.syntax.Commands.Add(this.command);
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

        public IJsonWriteModelSyntax WithReader(string name = null, string nameSpace = null, string relativePath = null)
        {
            this.command.Parameters.WithReader = true;
            this.command.Parameters.ReaderName = name ?? this.command.Parameters.ModelName + "Reader";
            this.command.Parameters.ReaderNamespace = nameSpace ?? this.command.Parameters.ModelNamespace;
            this.command.Parameters.ReaderPath = relativePath ?? this.command.Parameters.ModelPath;
            return this;
        }
    }
}
