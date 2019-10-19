namespace KY.Generator.Tsql.Type
{
    public class TsqlParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public bool IsNullable { get; set; }
    }
}