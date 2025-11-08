namespace KY.Generator.Mappings
{
    public class ClassMapping
    {
        public string Name { get; }
        public string MappedName { get; }
        public bool Ignored { get; }

        public ClassMapping(string name)
        {
            this.Name = name;
            this.Ignored = true;
        }

        public ClassMapping(string name, string mappedName)
        {
            this.Name = name;
            this.MappedName = mappedName;
        }
    }
}