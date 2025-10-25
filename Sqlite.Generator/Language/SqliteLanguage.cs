using KY.Generator.Languages;

namespace KY.Generator.Sqlite.Language
{
    public class SqliteLanguage : EmptyLanguage
    {
        public static SqliteLanguage Instance { get; } = new SqliteLanguage();

        public override string Name => "Sqlite";

        private SqliteLanguage()
        { }
    }
}
