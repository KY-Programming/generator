using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KY.Generator.Extensions;

namespace KY.Generator.Sqlite.Parsers
{
    public static class SqliteParser
    {
        private static readonly Regex createTableRegex = new(@"^\s*create\s+table\s+(?<name>[^\s(]+)\s*\((\s*(?<column>[^,]+)\s*,?)+\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex createTableColumnRegex = new(@"^\s*(?<name>[^\s]+)\s+(?<type>[^\s]+)\s*(?<notNull>not\s+null)?\s*(constraint\s+(?<constraint>)[^\s]+)?\s*(?<primaryKey>primary\skey)?\s*(?<autoIncrement>autoincrement)?");

        public static IEnumerable<ISqliteStatement> Parse(string code)
        {
            int createTableIndex = code.IndexOf(new Regex(@"create\s+table", RegexOptions.IgnoreCase));
            if (createTableIndex >= 0)
            {
                yield return ParseCreateTable(code.Substring(createTableIndex));
            }
        }

        private static ISqliteStatement ParseCreateTable(string substring)
        {
            Match match = createTableRegex.Match(substring);
            if (!match.Success)
            {
                throw new InvalidOperationException("Invalid create table syntax. Could not parse command. Should look like 'create table mytable (...)'");
            }
            SqliteCreateTable statement = new(match.Groups["name"].Value);
            foreach (Capture capture in match.Groups["column"].Captures)
            {
                Match columnMatch = createTableColumnRegex.Match(capture.Value);
                if (!columnMatch.Success)
                {
                    throw new InvalidOperationException($"Invalid create table syntax. Could not parse '{capture.Value}'");
                }
                statement.Columns.Add(new SqliteCreateTableColumn(
                                          columnMatch.Groups["name"].Value,
                                          columnMatch.Groups["type"].Value,
                                          string.IsNullOrEmpty(columnMatch.Groups["notNull"].Value),
                                          !string.IsNullOrEmpty(columnMatch.Groups["primaryKey"].Value),
                                          !string.IsNullOrEmpty(columnMatch.Groups["autoIncrement"].Value)
                                      ));
            }
            return statement;
        }
    }
}
