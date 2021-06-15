using KY.Generator.Syntax;
using KY.Generator.Tsql.Commands;

namespace KY.Generator
{
    public static class ReadFluentSyntaxExtension
    {
        public static ITsqlReadTableSyntax Tsql(this IReadFluentSyntax syntax, string connectionString)
        {
            return new TsqlReadSyntax((IReadFluentSyntaxInternal)syntax, connectionString);
        }
    }

    public interface ITsqlReadTableSyntax
    {
        ITsqlReadTableOrSwitchToWriteSyntax FromTable(string schema, string table);
    }

    public interface ITsqlReadTableOrSwitchToWriteSyntax : ITsqlReadTableSyntax, ISwitchToWriteSyntax
    {

    }

    public class TsqlReadSyntax : ITsqlReadTableSyntax, ITsqlReadTableOrSwitchToWriteSyntax
    {
        private readonly IReadFluentSyntaxInternal syntax;
        private readonly string connectionString;

        public TsqlReadSyntax(IReadFluentSyntaxInternal syntax, string connectionString)
        {
            this.syntax = syntax;
            this.connectionString = connectionString;
        }

        public ITsqlReadTableOrSwitchToWriteSyntax FromTable(string schema, string table)
        {
            TsqlReadCommand command = new TsqlReadCommand(this.syntax.Resolver);
            command.Parameters.ConnectionString = this.connectionString;
            command.Parameters.Schema = schema;
            command.Parameters.Table = table;
            this.syntax.Commands.Add(command);
            return this;
        }

        public IWriteFluentSyntax Write()
        {
            return this.syntax.Write();
        }
    }
}