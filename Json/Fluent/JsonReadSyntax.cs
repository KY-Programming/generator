using KY.Generator.Json.Commands;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public class JsonReadSyntax : IJsonReadSyntax, ISwitchToWriteSyntax
    {
        private readonly IReadFluentSyntaxInternal syntax;

        public JsonReadSyntax(IReadFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public ISwitchToWriteSyntax FromFile(string relativePath)
        {
            JsonReadCommand command = new JsonReadCommand(this.syntax.Resolver);
            command.Parameters.RelativePath = relativePath;
            this.syntax.Commands.Add(command);
            return this;
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax.Write();
        }
    }
}