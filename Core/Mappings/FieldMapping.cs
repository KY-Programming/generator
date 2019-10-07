namespace KY.Generator.Mappings
{
    public class FieldMapping
    {
        public string Name { get; }
        public string MappedName { get; }
        public bool Ignored { get; }

        public FieldMapping(string name)
        {
            this.Name = name;
            this.Ignored = true;
        }

        public FieldMapping(string name, string mappedName)
        {
            this.Name = name;
            this.MappedName = mappedName;
        }
    }
}