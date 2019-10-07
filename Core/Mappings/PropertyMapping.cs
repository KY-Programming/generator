namespace KY.Generator.Mappings
{
    public class PropertyMapping
    {
        public string Name { get; }
        public string MappedName { get; }
        public bool Ignored { get; }

        public PropertyMapping(string name)
        {
            this.Name = name;
            this.Ignored = true;
        }

        public PropertyMapping(string name, string mappedName)
        {
            this.Name = name;
            this.MappedName = mappedName;
        }
    }
}