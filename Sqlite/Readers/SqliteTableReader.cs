using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Sqlite.Commands;
using KY.Generator.Sqlite.Language;
using KY.Generator.Sqlite.Parsers;
using KY.Generator.Transfer;
using Microsoft.Data.Sqlite;

namespace KY.Generator.Sqlite.Readers
{
    public class SqliteTableReader
    {
        private static readonly Regex connectionStringRegex = new Regex(@".*Data\sSource=(?<file>[^;]*).*");

        public void Read(SqliteReadDatabaseCommandParameters parameters, List<ITransferObject> transferObjects, string outputPath)
        {
            Match match = connectionStringRegex.Match(parameters.ConnectionString);
            match.Success.AssertIs(true, "ConnectionString", "is not in valid format. Has to be like 'Data Source=my.db'");
            string originalFile = match.Groups["file"].Value;
            string file = originalFile;
            Logger.Trace($"Try to find sqlite database '{originalFile}'");
            if (!FileSystem.FileExists(file))
            {
                file = FileSystem.Combine(outputPath, originalFile);
                Logger.Trace($"Try to find sqlite database '{file}'");
            }
            if (!FileSystem.FileExists(file))
            {
                string fileName = FileSystem.GetFileName(file);
                Logger.Trace($"Start wildcard search for {fileName}");
                string[] files = FileSystem.GetFiles(outputPath, fileName, SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    file = files.First();
                    Logger.Trace($"Found in {file}");
                }
            }
            if (!FileSystem.FileExists(file))
            {
                throw new FileNotFoundException("File not found. File has to be in source or output directory", originalFile);
            }
            string connectionString = parameters.ConnectionString.Replace(originalFile, file);
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            List<string> tableNames = parameters.ReadAll ? this.ReadTables(connection).ToList() : parameters.Tables;
            List<SqliteCreateTable> statements = tableNames.SelectMany(tableName =>
            {
                string sql = this.ReadTableSql(connection, tableName);
                return SqliteParser.Parse(sql).OfType<SqliteCreateTable>();
            }).ToList();
            foreach (SqliteCreateTable statement in statements)
            {
                ModelTransferObject model = new()
                                            {
                                                Name = statement.TableName,
                                                Language = SqliteLanguage.Instance
                                            };
                foreach (SqliteCreateTableColumn column in statement.Columns)
                {
                    model.Fields.Add(new FieldTransferObject
                                     {
                                         Name = column.Name,
                                         Type = new ()
                                                {
                                                    Name = column.Type,
                                                    IsNullable = column.Nullable
                                                }
                                     });
                }
                transferObjects.Add(model);
            }
        }

        private IEnumerable<string> ReadTables(SqliteConnection connection)
        {
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%';";
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return reader.GetString(0);
            }
        }

        private string ReadTableSql(SqliteConnection connection, string tableName)
        {
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT sql FROM sqlite_master WHERE type ='table' AND name = @name;";
            command.Parameters.Add(new SqliteParameter("@name", tableName));
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return reader.GetString(0);
            }
            return null;
        }
    }
}
