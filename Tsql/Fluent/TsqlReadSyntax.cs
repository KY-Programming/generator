using System.Collections.Generic;
using KY.Generator.Command;
using KY.Generator.Syntax;
using KY.Generator.Tsql.Commands;

namespace KY.Generator.Tsql.Fluent
{
    public class TsqlReadSyntax : ITsqlReadSyntax, IExecutableSyntax
    {
        private readonly IReadFluentSyntaxInternal syntax;
        private readonly string connectionString;

        public List<IGeneratorCommand> Commands { get; } = new();

        public TsqlReadSyntax(IReadFluentSyntaxInternal syntax, string connectionString)
        {
            this.syntax = syntax;
            this.connectionString = connectionString;
        }

        public ITsqlReadSyntax FromTable(string schema, string table)
        {
            TsqlReadCommand command = this.syntax.Resolver.Create<TsqlReadCommand>();
            command.Parameters.ConnectionString = this.connectionString;
            command.Parameters.Schema = schema;
            command.Parameters.Table = table;
            this.Commands.Add(command);
            return this;
        }
    }
}
