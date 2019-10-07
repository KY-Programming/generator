namespace KY.Generator.Templates
{
    public class EnumValueTemplate
    {
        public string Name { get; }
        public ICodeFragment Value { get; }
        public string FormattedName { get; }

        public EnumValueTemplate(string name, ICodeFragment value, string formattedName = default)
        {
            this.Name = name;
            this.Value = value;
            this.FormattedName = formattedName ?? name;
        }
    }
}