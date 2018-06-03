namespace KY.Generator.Templates
{
    public class EnumValueTemplate
    {
        public string Name { get; }
        public ICodeFragment Value { get; }

        public EnumValueTemplate(string name, ICodeFragment value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}