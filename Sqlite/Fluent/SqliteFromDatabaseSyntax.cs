using KY.Generator.Command;
using KY.Generator.Sqlite.Commands;

namespace KY.Generator.Sqlite.Fluent
{
    public class SqliteFromDatabaseSyntax : ISqliteFromDatabaseOrReadSyntax
    {
        private readonly SqliteReadSyntax syntax;
        private readonly SqliteReadDatabaseCommand command;

        public SqliteFromDatabaseSyntax(SqliteReadSyntax syntax, string connectionString)
        {
            this.syntax = syntax;
            this.command = this.syntax.Resolver.Create<SqliteReadDatabaseCommand>();
            this.command.Parameters.ConnectionString = connectionString;
            this.syntax.Commands.Add(command);
        }

        public ISqliteFromDatabaseOrReadSyntax UseTable(string tableName)
        {
            this.command.Parameters.Tables.Add(tableName);
            return this;
        }

        public ISqliteFromDatabaseOrReadSyntax UseAll()
        {
            this.command.Parameters.ReadAll = true;
            return this;
        }

        public ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString)
        {
            return this.syntax.UseConnectionString(connectionString);
        }

        public ISqliteFromDatabaseOrReadSyntax UseFile(string file)
        {
            return this.syntax.UseFile(file);
        }
    }
}
