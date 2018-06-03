namespace KY.Generator.Tsql.Entity
{
    public class TsqlEnum
    {
        public TsqlEntity Entity { get; set; }
        public string RelativePath { get; set; }
        public string Namespace { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
    }
}