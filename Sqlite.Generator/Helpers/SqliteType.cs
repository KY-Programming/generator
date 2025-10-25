using OriginalTypes = Microsoft.Data.Sqlite.SqliteType;

namespace KY.Generator.Sqlite.Helpers
{
    public static class SqliteType
    {
        public static string Blob = nameof(OriginalTypes.Blob).ToUpper();
        public static string Integer = nameof(OriginalTypes.Integer).ToUpper();
        public static string Text = nameof(OriginalTypes.Text).ToUpper();
        public static string Real = nameof(OriginalTypes.Real).ToUpper();
        public static string Numeric = "NUMERIC";
    }
}
