using System.Collections;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Syntax;

namespace KY.Generator.Sqlite.Fluent
{
    public class SqliteReadSyntax : ISqliteReadSyntax
    {
        private readonly IReadFluentSyntaxInternal syntax;

        public IDependencyResolver Resolver => this.syntax.Resolver;
        public IList<IGeneratorCommand> Commands => this.syntax.Commands;

        public SqliteReadSyntax(IReadFluentSyntaxInternal syntax)
        {
            this.syntax = syntax;
        }

        public ISqliteFromDatabaseOrReadSyntax UseConnectionString(string connectionString)
        {
            return new SqliteFromDatabaseSyntax(this, connectionString);
        }

        public ISqliteFromDatabaseOrReadSyntax UseFile(string file)
        {
            return new SqliteFromDatabaseSyntax(this, $"Data Source={file}");
        }
    }
}
