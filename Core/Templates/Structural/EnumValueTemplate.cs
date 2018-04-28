namespace KY.Generator.Templates
{
    public class EnumValueTemplate
    {
        public string Name { get; }
        public CodeFragment Value { get; }

        public EnumValueTemplate(string name, CodeFragment value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}