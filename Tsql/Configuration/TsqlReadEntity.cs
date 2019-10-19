namespace KY.Generator.Tsql.Configuration
{
    public class TsqlReadEntity
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string StoredProcedure { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
    }
}