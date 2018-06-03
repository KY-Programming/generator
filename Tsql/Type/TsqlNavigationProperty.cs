namespace KY.Generator.Tsql.Type
{
    public class TsqlNavigationProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public TsqlNavigationProperty(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}