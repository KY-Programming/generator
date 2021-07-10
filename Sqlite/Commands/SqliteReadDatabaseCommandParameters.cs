using System.Collections.Generic;
using KY.Generator.Command;

namespace KY.Generator.Sqlite.Commands
{
    public class SqliteReadDatabaseCommandParameters : GeneratorCommandParameters
    {
        public string ConnectionString { get; set; }
        public bool ReadAll { get; set; }
        public List<string> Tables { get; set; } = new ();
    }
}
